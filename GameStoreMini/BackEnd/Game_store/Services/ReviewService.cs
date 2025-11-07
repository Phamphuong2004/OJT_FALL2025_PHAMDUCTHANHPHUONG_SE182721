using Game_store.Dtos;
using Game_store.Models;
using GameStoreMini.Data;
using GameStoreMini.Models;
using Microsoft.EntityFrameworkCore;

namespace Game_store.Services
{
    public interface IReviewService
    {
        Task<ReviewDto> CreateReviewAsync(int userId, CreateReviewDto dto);
        Task<ReviewDto> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDto dto);
        Task<bool> DeleteReviewAsync(int userId, int reviewId);
        Task<ReviewDto?> GetReviewByIdAsync(int reviewId, int? currentUserId = null);
        Task<(List<ReviewDto> Reviews, int TotalCount)> GetReviewsAsync(ReviewFilterDto filter, int? currentUserId = null);
        Task<bool> ToggleHelpfulAsync(int userId, int reviewId);
        Task<RatingStatisticsDto> GetRatingStatisticsAsync(int gameId);
        Task<bool> HasUserPurchasedGameAsync(int userId, int gameId);
    }

    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewDto> CreateReviewAsync(int userId, CreateReviewDto dto)
        {
            // Kiểm tra user đã review game này chưa
            var existingReview = await _context.ReviewGames
                .FirstOrDefaultAsync(r => r.UserId == userId && r.GameId == dto.GameId);

            if (existingReview != null)
            {
                throw new InvalidOperationException("Bạn đã đánh giá game này rồi");
            }

            // Kiểm tra game có tồn tại không
            var game = await _context.Games.FindAsync(dto.GameId);
            if (game == null)
            {
                throw new InvalidOperationException("Game không tồn tại");
            }

            // Kiểm tra user đã mua game chưa
            var hasPurchased = await HasUserPurchasedGameAsync(userId, dto.GameId);

            var review = new ReviewGame
            {
                GameId = dto.GameId,
                UserId = userId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                IsVerifiedPurchase = hasPurchased,
                CreatedAt = DateTime.UtcNow
            };

            _context.ReviewGames.Add(review);
            await _context.SaveChangesAsync();

            // Cập nhật rating của game
            await UpdateGameRatingAsync(dto.GameId);

            return await GetReviewByIdAsync(review.ReviewId, userId) 
                   ?? throw new InvalidOperationException("Không thể lấy review vừa tạo");
        }

        public async Task<ReviewDto> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDto dto)
        {
            var review = await _context.ReviewGames.FindAsync(reviewId);

            if (review == null)
            {
                throw new InvalidOperationException("Review không tồn tại");
            }

            if (review.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền sửa review này");
            }

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            review.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Cập nhật rating của game
            await UpdateGameRatingAsync(review.GameId);

            return await GetReviewByIdAsync(reviewId, userId) 
                   ?? throw new InvalidOperationException("Không thể lấy review vừa cập nhật");
        }

        public async Task<bool> DeleteReviewAsync(int userId, int reviewId)
        {
            var review = await _context.ReviewGames.FindAsync(reviewId);

            if (review == null)
            {
                return false;
            }

            if (review.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa review này");
            }

            var gameId = review.GameId;
            _context.ReviewGames.Remove(review);
            await _context.SaveChangesAsync();

            // Cập nhật rating của game
            await UpdateGameRatingAsync(gameId);

            return true;
        }

        public async Task<ReviewDto?> GetReviewByIdAsync(int reviewId, int? currentUserId = null)
        {
            var review = await _context.ReviewGames
                .Include(r => r.User)
                .Include(r => r.Game)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);

            if (review == null)
            {
                return null;
            }

            bool? isHelpful = null;
            if (currentUserId.HasValue)
            {
                isHelpful = await _context.ReviewHelpfuls
                    .AnyAsync(rh => rh.ReviewId == reviewId && rh.UserId == currentUserId.Value);
            }

            return new ReviewDto
            {
                ReviewId = review.ReviewId,
                GameId = review.GameId,
                GameTitle = review.Game?.Title,
                UserId = review.UserId,
                UserName = review.User?.UserName ?? "Guest",
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
                IsVerifiedPurchase = review.IsVerifiedPurchase,
                HelpfulCount = review.HelpfulCount,
                IsHelpfulByCurrentUser = isHelpful
            };
        }

        public async Task<(List<ReviewDto> Reviews, int TotalCount)> GetReviewsAsync(
            ReviewFilterDto filter, int? currentUserId = null)
        {
            var query = _context.ReviewGames
                .Include(r => r.User)
                .Include(r => r.Game)
                .AsQueryable();

            // Áp dụng filters
            if (filter.GameId.HasValue)
            {
                query = query.Where(r => r.GameId == filter.GameId.Value);
            }

            if (filter.UserId.HasValue)
            {
                query = query.Where(r => r.UserId == filter.UserId.Value);
            }

            if (filter.MinRating.HasValue)
            {
                query = query.Where(r => r.Rating >= filter.MinRating.Value);
            }

            if (filter.MaxRating.HasValue)
            {
                query = query.Where(r => r.Rating <= filter.MaxRating.Value);
            }

            if (filter.VerifiedPurchaseOnly == true)
            {
                query = query.Where(r => r.IsVerifiedPurchase);
            }

            // Đếm tổng số
            var totalCount = await query.CountAsync();

            // Sắp xếp
            query = filter.SortBy?.ToLower() switch
            {
                "rating" => filter.SortOrder?.ToLower() == "asc" 
                    ? query.OrderBy(r => r.Rating) 
                    : query.OrderByDescending(r => r.Rating),
                "helpful" => filter.SortOrder?.ToLower() == "asc" 
                    ? query.OrderBy(r => r.HelpfulCount) 
                    : query.OrderByDescending(r => r.HelpfulCount),
                _ => filter.SortOrder?.ToLower() == "asc" 
                    ? query.OrderBy(r => r.CreatedAt) 
                    : query.OrderByDescending(r => r.CreatedAt)
            };

            // Phân trang
            var reviews = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            // Lấy thông tin helpful của user hiện tại
            List<int>? helpfulReviewIds = null;
            if (currentUserId.HasValue)
            {
                var reviewIds = reviews.Select(r => r.ReviewId).ToList();
                helpfulReviewIds = await _context.ReviewHelpfuls
                    .Where(rh => reviewIds.Contains(rh.ReviewId) && rh.UserId == currentUserId.Value)
                    .Select(rh => rh.ReviewId)
                    .ToListAsync();
            }

            var reviewDtos = reviews.Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                GameId = r.GameId,
                GameTitle = r.Game?.Title,
                UserId = r.UserId,
                UserName = r.User?.UserName ?? "Guest",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                IsVerifiedPurchase = r.IsVerifiedPurchase,
                HelpfulCount = r.HelpfulCount,
                IsHelpfulByCurrentUser = helpfulReviewIds?.Contains(r.ReviewId)
            }).ToList();

            return (reviewDtos, totalCount);
        }

        public async Task<bool> ToggleHelpfulAsync(int userId, int reviewId)
        {
            var review = await _context.ReviewGames.FindAsync(reviewId);
            if (review == null)
            {
                throw new InvalidOperationException("Review không tồn tại");
            }

            var existing = await _context.ReviewHelpfuls
                .FirstOrDefaultAsync(rh => rh.ReviewId == reviewId && rh.UserId == userId);

            if (existing != null)
            {
                // Remove helpful
                _context.ReviewHelpfuls.Remove(existing);
                review.HelpfulCount--;
            }
            else
            {
                // Add helpful
                _context.ReviewHelpfuls.Add(new ReviewHelpful
                {
                    ReviewId = reviewId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                });
                review.HelpfulCount++;
            }

            await _context.SaveChangesAsync();
            return existing == null; // Return true if added, false if removed
        }

        public async Task<RatingStatisticsDto> GetRatingStatisticsAsync(int gameId)
        {
            var reviews = await _context.ReviewGames
                .Where(r => r.GameId == gameId)
                .ToListAsync();

            var stats = new RatingStatisticsDto
            {
                GameId = gameId,
                TotalReviews = reviews.Count,
                AverageRating = reviews.Any() ? (decimal)reviews.Average(r => r.Rating) : 0m,
                RatingDistribution = new Dictionary<int, int>
                {
                    { 5, reviews.Count(r => r.Rating == 5) },
                    { 4, reviews.Count(r => r.Rating == 4) },
                    { 3, reviews.Count(r => r.Rating == 3) },
                    { 2, reviews.Count(r => r.Rating == 2) },
                    { 1, reviews.Count(r => r.Rating == 1) }
                }
            };

            return stats;
        }

        public async Task<bool> HasUserPurchasedGameAsync(int userId, int gameId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .AnyAsync(o => o.UserId == userId && 
                              o.Status == "Completed" &&
                              o.Items.Any(oi => oi.GameId == gameId));
        }

        private async Task UpdateGameRatingAsync(int gameId)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game == null) return;

            var reviews = await _context.ReviewGames
                .Where(r => r.GameId == gameId)
                .ToListAsync();

            game.ReviewCount = reviews.Count;
            game.AverageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

            await _context.SaveChangesAsync();
        }
    }
}