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
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WishlistController(AppDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        // GET: api/wishlist
        [HttpGet]
        public async Task<ActionResult<List<WishlistItemDto>>> GetWishlist()
        {
            var userId = GetCurrentUserId();

            var wishlist = await _context.Set<Wishlist>()
                .Where(w => w.UserId == userId)
                .Include(w => w.Game)
                    .ThenInclude(g => g.Categories)
                .OrderByDescending(w => w.AddedAt)
                .Select(w => new WishlistItemDto
                {
                    WishlistId = w.WishlistId,
                    GameId = w.GameId,
                    GameTitle = w.Game.Title,
                    GamePrice = w.Game.Price,
                    GameImageUrl = w.Game.ImageUrl,
                    GameDescription = w.Game.Description,
                    CategoryName = w.Game.Categories.FirstOrDefault() != null ? w.Game.Categories.FirstOrDefault()!.Name : null,
                    AverageRating = w.Game.AverageRating,
                    ReviewCount = w.Game.ReviewCount,
                    AddedAt = w.AddedAt
                })
                .ToListAsync();

            return Ok(wishlist);
        }

        // POST: api/wishlist/{gameId}
        [HttpPost("{gameId}")]
        public async Task<ActionResult> AddToWishlist(int gameId)
        {
            var userId = GetCurrentUserId();

            // Check if game exists
            var game = await _context.Games.FindAsync(gameId);
            if (game == null)
            {
                return NotFound(new { message = "Game không tồn tại" });
            }

            // Check if already in wishlist
            var existing = await _context.Set<Wishlist>()
                .FirstOrDefaultAsync(w => w.UserId == userId && w.GameId == gameId);

            if (existing != null)
            {
                return BadRequest(new { message = "Game đã có trong wishlist" });
            }

            var wishlistItem = new Wishlist
            {
                UserId = userId,
                GameId = gameId,
                AddedAt = DateTime.UtcNow
            };

            _context.Set<Wishlist>().Add(wishlistItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã thêm vào wishlist", wishlistId = wishlistItem.WishlistId });
        }

        // DELETE: api/wishlist/{gameId}
        [HttpDelete("{gameId}")]
        public async Task<ActionResult> RemoveFromWishlist(int gameId)
        {
            var userId = GetCurrentUserId();

            var wishlistItem = await _context.Set<Wishlist>()
                .FirstOrDefaultAsync(w => w.UserId == userId && w.GameId == gameId);

            if (wishlistItem == null)
            {
                return NotFound(new { message = "Game không có trong wishlist" });
            }

            _context.Set<Wishlist>().Remove(wishlistItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa khỏi wishlist" });
        }

        // GET: api/wishlist/check/{gameId}
        [HttpGet("check/{gameId}")]
        public async Task<ActionResult<bool>> CheckInWishlist(int gameId)
        {
            var userId = GetCurrentUserId();

            var exists = await _context.Set<Wishlist>()
                .AnyAsync(w => w.UserId == userId && w.GameId == gameId);

            return Ok(new { inWishlist = exists });
        }

        // GET: api/wishlist/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetWishlistCount()
        {
            var userId = GetCurrentUserId();

            var count = await _context.Set<Wishlist>()
                .CountAsync(w => w.UserId == userId);

            return Ok(new { count });
        }
    }
}
