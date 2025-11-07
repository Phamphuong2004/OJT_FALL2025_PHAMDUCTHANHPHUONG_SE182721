using GameStoreMini.Data;
using Game_store.Dtos;
using Game_store.Models;
using Game_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Game_store.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IReviewService _reviewService;

        public ReviewsController(AppDbContext context, IReviewService reviewService)
        {
            _context = context;
            _reviewService = reviewService;
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        // GET: api/reviews
        [HttpGet]
        public async Task<ActionResult> GetReviews(
            [FromQuery] int? gameId,
            [FromQuery] int? userId,
            [FromQuery] int? minRating,
            [FromQuery] int? maxRating,
            [FromQuery] bool? verifiedPurchaseOnly,
            [FromQuery] string? sortBy = "date",
            [FromQuery] string? sortOrder = "desc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                
                var filter = new ReviewFilterDto
                {
                    GameId = gameId,
                    UserId = userId,
                    MinRating = minRating,
                    MaxRating = maxRating,
                    VerifiedPurchaseOnly = verifiedPurchaseOnly,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    Page = page,
                    PageSize = pageSize
                };

                var (reviews, totalCount) = await _reviewService.GetReviewsAsync(filter, currentUserId);

                var result = new
                {
                    data = reviews,
                    page = page,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tải reviews", error = ex.Message });
            }
        }

        // GET: api/reviews/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReviewById(int id)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var review = await _reviewService.GetReviewByIdAsync(id, currentUserId);

                if (review == null)
                {
                    return NotFound(new { message = "Review không tồn tại" });
                }

                return Ok(review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tải review", error = ex.Message });
            }
        }

        // GET: api/reviews/game/{gameId}/statistics
        [HttpGet("game/{gameId}/statistics")]
        public async Task<ActionResult<RatingStatisticsDto>> GetGameStatistics(int gameId)
        {
            try
            {
                var stats = await _reviewService.GetRatingStatisticsAsync(gameId);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tải thống kê", error = ex.Message });
            }
        }

        // POST: api/reviews
        [HttpPost]
        [Authorize] // Chỉ cho phép user đã login
        public async Task<ActionResult<ReviewDto>> CreateReview([FromBody] CreateReviewDto createDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized(new { message = "Vui lòng đăng nhập" });
                }

                // Validate
                if (createDto.Rating < 1 || createDto.Rating > 5)
                {
                    return BadRequest(new { message = "Rating phải từ 1 đến 5 sao" });
                }

                if (string.IsNullOrWhiteSpace(createDto.Comment))
                {
                    return BadRequest(new { message = "Vui lòng nhập nội dung đánh giá" });
                }

                // Kiểm tra user đã mua game chưa
                var hasPurchased = await _reviewService.HasUserPurchasedGameAsync(userId.Value, createDto.GameId);
                if (!hasPurchased)
                {
                    return BadRequest(new { message = "Bạn cần mua game này trước khi đánh giá" });
                }

                // Kiểm tra user đã review chưa
                var existingReview = await _context.ReviewGames
                    .FirstOrDefaultAsync(r => r.UserId == userId.Value && r.GameId == createDto.GameId);

                if (existingReview != null)
                {
                    return BadRequest(new { message = "Bạn đã đánh giá game này rồi" });
                }

                var review = await _reviewService.CreateReviewAsync(userId.Value, createDto);
                return CreatedAtAction(nameof(GetReviewById), new { id = review.ReviewId }, review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo review", error = ex.Message });
            }
        }

        // PUT: api/reviews/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ReviewDto>> UpdateReview(int id, [FromBody] UpdateReviewDto updateDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized(new { message = "Vui lòng đăng nhập" });
                }

                var existingReview = await _context.ReviewGames.FindAsync(id);
                if (existingReview == null)
                {
                    return NotFound(new { message = "Review không tồn tại" });
                }

                // Kiểm tra quyền sở hữu
                if (existingReview.UserId != userId.Value)
                {
                    return Forbid(); // 403
                }

                // Validate
                if (updateDto.Rating < 1 || updateDto.Rating > 5)
                {
                    return BadRequest(new { message = "Rating phải từ 1 đến 5 sao" });
                }

                var updatedReview = await _reviewService.UpdateReviewAsync(userId.Value, id, updateDto);
                return Ok(updatedReview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật review", error = ex.Message });
            }
        }

        // DELETE: api/reviews/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteReview(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized(new { message = "Vui lòng đăng nhập" });
                }

                var existingReview = await _context.ReviewGames.FindAsync(id);
                if (existingReview == null)
                {
                    return NotFound(new { message = "Review không tồn tại" });
                }

                // Kiểm tra quyền: Chỉ owner hoặc admin
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                if (existingReview.UserId != userId.Value && userRole != "Admin")
                {
                    return Forbid();
                }

                await _reviewService.DeleteReviewAsync(userId.Value, id);
                return Ok(new { message = "Đã xóa review" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa review", error = ex.Message });
            }
        }

        // POST: api/reviews/{id}/helpful
        [HttpPost("{id}/helpful")]
        [Authorize]
        public async Task<ActionResult> ToggleHelpful(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized(new { message = "Vui lòng đăng nhập" });
                }

                var review = await _context.ReviewGames.FindAsync(id);
                if (review == null)
                {
                    return NotFound(new { message = "Review không tồn tại" });
                }

                var isMarked = await _reviewService.ToggleHelpfulAsync(userId.Value, id);
                
                return Ok(new 
                { 
                    message = isMarked ? "Đã đánh dấu hữu ích" : "Đã bỏ đánh dấu",
                    isMarked = isMarked,
                    helpfulCount = review.HelpfulCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi đánh dấu helpful", error = ex.Message });
            }
        }

        // GET: api/reviews/user/can-review/{gameId}
        [HttpGet("user/can-review/{gameId}")]
        [Authorize]
        public async Task<ActionResult> CanUserReviewGame(int gameId)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized(new { message = "Vui lòng đăng nhập" });
                }

                // Kiểm tra đã mua chưa
                var hasPurchased = await _reviewService.HasUserPurchasedGameAsync(userId.Value, gameId);
                if (!hasPurchased)
                {
                    return Ok(new 
                    { 
                        canReview = false, 
                        reason = "Bạn cần mua game này trước khi đánh giá" 
                    });
                }

                // Kiểm tra đã review chưa
                var hasReviewed = await _context.ReviewGames
                    .AnyAsync(r => r.UserId == userId.Value && r.GameId == gameId);

                if (hasReviewed)
                {
                    return Ok(new 
                    { 
                        canReview = false, 
                        reason = "Bạn đã đánh giá game này rồi" 
                    });
                }

                return Ok(new { canReview = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi kiểm tra quyền", error = ex.Message });
            }
        }
    }
}
