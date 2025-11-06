using Microsoft.AspNetCore.Mvc;
using Game_store.Models;
using Microsoft.EntityFrameworkCore;
using GameStoreMini.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Game_store.Dtos;

[ApiController]
[Route("api/promotion")]
public class PromotionController : ControllerBase
{
    private readonly AppDbContext _context;

    private int? CurrentUserId
    {
        get
        {
            var sub = User?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(sub)) return null;
            if (int.TryParse(sub, out var id)) return id;
            return null;
        }
    }

    public PromotionController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/promotion/{id}/claim - authenticated customers can claim an active promotion
    [HttpPost("{id}/claim")]
    [Authorize]
    public async Task<IActionResult> ClaimPromotion([FromRoute] int id)
    {
        if (CurrentUserId == null) return Unauthorized();

        var promotion = await _context.Promotions.FindAsync(id);
        if (promotion == null) return NotFound("Promotion not found.");

        var now = DateTime.UtcNow;
        if (!promotion.IsActive || promotion.StartDate > now || promotion.EndDate < now)
            return BadRequest("Promotion is not active.");

        // Prevent duplicate claims by same user for same promotion
        var existing = await _context.PromotionClaims
            .FirstOrDefaultAsync(pc => pc.PromotionId == id && pc.UserId == CurrentUserId.Value);
        if (existing != null)
        {
            return BadRequest(new { message = "Bạn đã nhận khuyến mãi này rồi.", claimId = existing.Id });
        }

        var claim = new Game_store.Models.PromotionClaim
        {
            PromotionId = id,
            UserId = CurrentUserId.Value,
            ClaimedAt = DateTime.UtcNow
        };

        _context.PromotionClaims.Add(claim);
        await _context.SaveChangesAsync();

        var dto = new PromotionClaimDto
        {
            Id = claim.Id,
            PromotionId = claim.PromotionId,
            UserId = claim.UserId,
            ClaimedAt = claim.ClaimedAt,
            IsRedeemed = claim.IsRedeemed,
            RedeemedAt = claim.RedeemedAt,
            Notes = claim.Notes,
            PromotionTitle = promotion.Title,
            PromotionImageUrl = promotion.ImageUrl
        };

        return Ok(dto);
    }

    // GET: api/promotion/my-claims - list claims for current user
    [HttpGet("my-claims")]
    [Authorize]
    public async Task<IActionResult> GetMyClaims()
    {
        if (CurrentUserId == null) return Unauthorized();

        var claims = await _context.PromotionClaims
            .Where(pc => pc.UserId == CurrentUserId.Value)
            .Include(pc => pc.Promotion)
            .OrderByDescending(pc => pc.ClaimedAt)
            .Select(pc => new PromotionClaimDto
            {
                Id = pc.Id,
                PromotionId = pc.PromotionId,
                UserId = pc.UserId,
                ClaimedAt = pc.ClaimedAt,
                IsRedeemed = pc.IsRedeemed,
                RedeemedAt = pc.RedeemedAt,
                Notes = pc.Notes,
                PromotionTitle = pc.Promotion.Title,
                PromotionImageUrl = pc.Promotion.ImageUrl
            })
            .ToListAsync();

        return Ok(claims);
    }

    // GET: api/promotion - Chỉ lấy promotions đang active cho customer/guest
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetActivePromotions(
        [FromQuery] string? eventType = null,
        [FromQuery] bool? isFeatured = null,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        var now = DateTime.UtcNow;
        
        var query = _context.Promotions
            .Include(p => p.PromotionGames)
                .ThenInclude(pg => pg.Game)
            .Where(p => p.IsActive && p.StartDate <= now && p.EndDate >= now) // Chỉ lấy active và trong thời gian
            .AsQueryable();

        // Filters
        if (!string.IsNullOrEmpty(eventType))
            query = query.Where(p => p.EventType == eventType);

        if (isFeatured.HasValue)
            query = query.Where(p => p.IsFeatured == isFeatured.Value);

        var totalCount = await query.CountAsync();
        
        var promotions = await query
            .OrderByDescending(p => p.IsFeatured)
            .ThenByDescending(p => p.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Summary,
                p.ImageUrl,
                p.Slug,
                p.StartDate,
                p.EndDate,
                p.DiscountPercentage,
                p.FixedDiscountAmount,
                p.PromotionType,
                p.EventType,
                p.IsFeatured,
                p.CreatedAt,
                GamesCount = p.PromotionGames.Count()
            })
            .ToListAsync();

        return Ok(new
        {
            Data = promotions,
            TotalCount = totalCount,
            Page = page,
            Limit = limit,
            TotalPages = (int)Math.Ceiling((double)totalCount / limit)
        });
    }

    // Các methods khác giữ nguyên như trước...
    // GET: api/promotion/{id}
    // GET: api/promotion/slug/{slug}  
    // GET: api/promotion/featured
}