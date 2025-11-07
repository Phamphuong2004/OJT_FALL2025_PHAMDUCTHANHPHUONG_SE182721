using Game_store.Dtos;
using Game_store.Models;
using GameStoreMini.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Game_store.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Phải đăng nhập
    public class ViewHistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ViewHistoryController(AppDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        // GET: api/viewhistory
        [HttpGet]
        public async Task<ActionResult<ViewHistoryListResult>> GetViewHistory(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized(new { message = "Vui lòng đăng nhập" });

            var query = _context.ViewHistories
                .Where(vh => vh.UserId == userId)
                .Include(vh => vh.Game)
                    .ThenInclude(g => g.Categories)
                .OrderByDescending(vh => vh.LastViewedAt ?? vh.ViewedAt);

            var totalCount = await query.CountAsync();

            var viewHistory = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(vh => new ViewHistoryDtos
                {
                    ViewHistoryId = vh.ViewHistoryId,
                    GameId = vh.GameId,
                    GameTitle = vh.Game.Title,
                    GamePrice = vh.Game.Price,
                    GameImageUrl = vh.Game.ImageUrl,
                    GameDescription = vh.Game.Description,
                    CategoryName = vh.Game.Categories.FirstOrDefault() != null 
                        ? vh.Game.Categories.FirstOrDefault()!.Name 
                        : null,
                    AverageRating = vh.Game.AverageRating,
                    ReviewCount = vh.Game.ReviewCount,
                    ViewedAt = vh.ViewedAt,
                    LastViewedAt = vh.LastViewedAt,
                    ViewCount = vh.ViewCount
                })
                .ToListAsync();

            return Ok(new ViewHistoryListResult
            {
                Data = viewHistory,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }

        // POST: api/viewhistory
        [HttpPost]
        public async Task<ActionResult> AddViewHistory([FromBody] AddViewHistoryDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized(new { message = "Vui lòng đăng nhập" });

            // Check if game exists
            var game = await _context.Games.FindAsync(dto.GameId);
            if (game == null)
                return NotFound(new { message = "Game không tồn tại" });

            // Check if already viewed
            var existing = await _context.ViewHistories
                .FirstOrDefaultAsync(vh => vh.UserId == userId && vh.GameId == dto.GameId);

            if (existing != null)
            {
                // Update view count and last viewed time
                existing.ViewCount++;
                existing.LastViewedAt = DateTime.UtcNow;
            }
            else
            {
                // Create new view history
                var viewHistory = new ViewHistory
                {
                    UserId = userId,
                    GameId = dto.GameId,
                    ViewedAt = DateTime.UtcNow,
                    ViewCount = 1
                };
                _context.ViewHistories.Add(viewHistory);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã cập nhật lịch sử xem" });
        }

        // DELETE: api/viewhistory/{gameId}
        [HttpDelete("{gameId}")]
        public async Task<ActionResult> RemoveFromHistory(int gameId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized(new { message = "Vui lòng đăng nhập" });

            var viewHistory = await _context.ViewHistories
                .FirstOrDefaultAsync(vh => vh.UserId == userId && vh.GameId == gameId);

            if (viewHistory == null)
                return NotFound(new { message = "Không tìm thấy lịch sử xem" });

            _context.ViewHistories.Remove(viewHistory);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa khỏi lịch sử" });
        }

        // DELETE: api/viewhistory/clear
        [HttpDelete("clear")]
        public async Task<ActionResult> ClearHistory()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized(new { message = "Vui lòng đăng nhập" });

            var items = await _context.ViewHistories
                .Where(vh => vh.UserId == userId)
                .ToListAsync();

            _context.ViewHistories.RemoveRange(items);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa toàn bộ lịch sử xem" });
        }

        // GET: api/viewhistory/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetHistoryCount()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Ok(new { count = 0 });

            var count = await _context.ViewHistories
                .CountAsync(vh => vh.UserId == userId);

            return Ok(new { count });
        }
    }
}