using Game_store.Dtos;
using Game_store.Models;
using Game_store.Services;
using GameStoreMini.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game_store.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")] // Chỉ Admin mới truy cập được
    public class AdminReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IReviewService _reviewService;

        public AdminReviewsController(AppDbContext context, IReviewService reviewService)
        {
            _context = context;
            _reviewService = reviewService;
        }

        // GET: api/admin/adminreviews - Lấy TẤT CẢ reviews (có filter)
        [HttpGet]
        public async Task<ActionResult<AdminReviewListResult>> GetAllReviews(
            [FromQuery] int? gameId,
            [FromQuery] int? userId,
            [FromQuery] string? search,
            [FromQuery] int? minRating,
            [FromQuery] int? maxRating,
            [FromQuery] bool? verifiedPurchaseOnly,
            [FromQuery] string? sortBy = "date",
            [FromQuery] string? sortOrder = "desc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var query = _context.ReviewGames
                    .Include(r => r.User)
                    .Include(r => r.Game)
                    .AsQueryable();

                // Filters
                if (gameId.HasValue)
                    query = query.Where(r => r.GameId == gameId.Value);

                if (userId.HasValue)
                    query = query.Where(r => r.UserId == userId.Value);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(r =>
                        (r.Comment != null && r.Comment.Contains(search)) ||
                        (r.User != null && r.User.UserName != null && r.User.UserName.Contains(search)) ||
                        (r.Game != null && r.Game.Title.Contains(search))
                    );
                }

                if (minRating.HasValue)
                    query = query.Where(r => r.Rating >= minRating.Value);

                if (maxRating.HasValue)
                    query = query.Where(r => r.Rating <= maxRating.Value);

                if (verifiedPurchaseOnly == true)
                    query = query.Where(r => r.IsVerifiedPurchase);

                // Sorting
                query = sortBy?.ToLower() switch
                {
                    "rating" => sortOrder?.ToLower() == "asc"
                        ? query.OrderBy(r => r.Rating)
                        : query.OrderByDescending(r => r.Rating),
                    "helpful" => sortOrder?.ToLower() == "asc"
                        ? query.OrderBy(r => r.HelpfulCount)
                        : query.OrderByDescending(r => r.HelpfulCount),
                    "game" => sortOrder?.ToLower() == "asc"
                        ? query.OrderBy(r => r.Game != null ? r.Game.Title : "")
                        : query.OrderByDescending(r => r.Game != null ? r.Game.Title : ""),
                    "user" => sortOrder?.ToLower() == "asc"
                        ? query.OrderBy(r => r.User != null ? r.User.UserName : "")
                        : query.OrderByDescending(r => r.User != null ? r.User.UserName : ""),
                    _ => sortOrder?.ToLower() == "asc"
                        ? query.OrderBy(r => r.CreatedAt)
                        : query.OrderByDescending(r => r.CreatedAt)
                };

                var totalCount = await query.CountAsync();

                var reviews = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(r => new AdminReviewsDto
                    {
                        ReviewId = r.ReviewId,
                        GameId = r.GameId,
                        GameTitle = r.Game != null ? r.Game.Title : "",
                        GameImageUrl = r.Game != null ? r.Game.ImageUrl : null,
                        UserId = r.UserId,
                        Username = r.User != null ? r.User.UserName ?? "Unknown" : "Unknown",
                        UserEmail = r.User != null ? r.User.Email : null,
                        Rating = r.Rating,
                        Comment = r.Comment ?? "",
                        VerifiedPurchase = r.IsVerifiedPurchase,
                        HelpfulCount = r.HelpfulCount,
                        CreatedAt = r.CreatedAt,
                        UpdatedAt = r.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(new AdminReviewListResult
                {
                    Data = reviews,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tải reviews", error = ex.Message });
            }
        }

        // GET: api/admin/adminreviews/statistics - Thống kê tổng quan
        [HttpGet("statistics")]
        public async Task<ActionResult<AdminReviewsStatistics>> GetStatistics()
        {
            try
            {
                var totalReviews = await _context.ReviewGames.CountAsync();
                var averageRating = await _context.ReviewGames.AverageAsync(r => (double?)r.Rating) ?? 0;
                var verifiedReviews = await _context.ReviewGames.CountAsync(r => r.IsVerifiedPurchase);

                var ratingDistribution = await _context.ReviewGames
                    .GroupBy(r => r.Rating)
                    .Select(g => new { Rating = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Rating, x => x.Count);

                var topGames = await _context.ReviewGames
                    .Where(r => r.Game != null)
                    .GroupBy(r => new { r.GameId, Title = r.Game!.Title })
                    .Select(g => new TopReviewedGame
                    {
                        GameId = g.Key.GameId,
                        GameTitle = g.Key.Title,
                        ReviewCount = g.Count(),
                        AverageRating = g.Average(r => r.Rating)
                    })
                    .OrderByDescending(g => g.ReviewCount)
                    .Take(5)
                    .ToListAsync();

                return Ok(new AdminReviewsStatistics
                {
                    TotalReviews = totalReviews,
                    AverageRating = Math.Round(averageRating, 2),
                    VerifiedReviewsCount = verifiedReviews,
                    VerifiedReviewsPercentage = totalReviews > 0
                        ? Math.Round((verifiedReviews / (double)totalReviews) * 100, 1)
                        : 0,
                    RatingDistribution = ratingDistribution,
                    TopReviewedGames = topGames
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tải thống kê", error = ex.Message });
            }
        }

        // DELETE: api/admin/adminreviews/{id} - Admin xóa bất kỳ review nào
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(int id, [FromQuery] string? reason)
        {
            try
            {
                var review = await _context.ReviewGames.FindAsync(id);
                if (review == null)
                {
                    return NotFound(new { message = "Review không tồn tại" });
                }

                var gameId = review.GameId;

                _context.ReviewGames.Remove(review);
                await _context.SaveChangesAsync();

                // Update game statistics
                await UpdateGameRatingAsync(gameId);

                return Ok(new { message = "Đã xóa review", reason = reason });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa review", error = ex.Message });
            }
        }

        // PUT: api/admin/adminreviews/{id}/hide - Ẩn/hiện review
        [HttpPut("{id}/hide")]
        public async Task<ActionResult> ToggleHideReview(int id, [FromBody] HideReviewRequest request)
        {
            try
            {
                var review = await _context.ReviewGames.FindAsync(id);
                if (review == null)
                {
                    return NotFound(new { message = "Review không tồn tại" });
                }

                // Note: Bạn cần thêm field IsHidden, HiddenReason, HiddenAt vào model ReviewGame nếu muốn dùng tính năng này
                // review.IsHidden = request.IsHidden;
                // review.HiddenReason = request.Reason;
                // review.HiddenAt = request.IsHidden ? DateTime.UtcNow : null;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = request.IsHidden ? "Đã ẩn review" : "Đã hiện review",
                    isHidden = request.IsHidden
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi ẩn/hiện review", error = ex.Message });
            }
        }

        private async Task UpdateGameRatingAsync(int gameId)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game == null) return;

            var reviews = await _context.ReviewGames
                .Where(r => r.GameId == gameId)
                .ToListAsync();

            game.ReviewCount = reviews.Count;
            game.AverageRating = reviews.Any()
                ? Math.Round(reviews.Average(r => r.Rating), 1)
                : 0;

            await _context.SaveChangesAsync();
        }
    }
}