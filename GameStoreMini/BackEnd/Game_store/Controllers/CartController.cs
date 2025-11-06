using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using GameStoreMini.Data;
using GameStoreMini.Dtos;
using GameStoreMini.Models;
using Microsoft.AspNetCore.SignalR;
using GameStoreMini.Hubs;

namespace GameStoreMini.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CartController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IHubContext<CartHub> _hub;
        public CartController(AppDbContext db, IHubContext<CartHub> hub) => (_db, _hub) = (db, hub);

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

        private string? GetAnonymousId()
        {
            return Request.Cookies["anonymousId"] ?? Request.Headers["X-Anonymous-Id"].FirstOrDefault();
        }

            private string? MakeAbsolute(string? url)
            {
                if (string.IsNullOrWhiteSpace(url)) return null;
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute)) return url;
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                return baseUrl + (url.StartsWith("/") ? "" : "/") + url;
            }

        // GET: /api/cart
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = CurrentUserId;
            var anon = GetAnonymousId();

            Cart? cart = null;
            if (userId.HasValue)
            {
                cart = await _db.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Game)
                    .FirstOrDefaultAsync(c => c.UserId == userId.Value);
            }
            else if (!string.IsNullOrEmpty(anon))
            {
                cart = await _db.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Game)
                    .FirstOrDefaultAsync(c => c.AnonymousId == anon);
            }

            if (cart == null)
            {
                return Ok(new { items = Array.Empty<object>() });
            }

            string? MakeAbsolute(string? url)
            {
                if (string.IsNullOrWhiteSpace(url)) return null;
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute)) return url;
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                return baseUrl + (url.StartsWith("/") ? "" : "/") + url;
            }

            var result = cart.Items.Select(i => new
            {
                i.GameId,
                i.Quantity,
                Game = i.Game == null ? null : new
                {
                    i.Game.Id,
                    i.Game.Title,
                    i.Game.Price,
                    ImageUrl = MakeAbsolute(i.Game.ImageUrl)
                }
            });

            return Ok(new { items = result });
        }

        // POST: /api/cart/add
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            // helpful debug response when model binding fails or payload is invalid
            if (dto == null || dto.GameId <= 0 || dto.Quantity <= 0)
            {
                var anonHeader = Request.Headers["X-Anonymous-Id"].FirstOrDefault();
                return BadRequest(new
                {
                    message = "Invalid payload for AddToCart",
                    dtoIsNull = dto == null,
                    contentType = Request.ContentType,
                    contentLength = Request.ContentLength,
                    hasAuthorization = Request.Headers.ContainsKey("Authorization"),
                    anonymousId = anonHeader
                });
            }

            var userId = CurrentUserId;

            Cart? cart = null;
            string? anon = null;

            if (userId.HasValue)
            {
                cart = await _db.Carts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UserId == userId.Value);
            }
            else
            {
                anon = Request.Cookies["anonymousId"] ?? Request.Headers["X-Anonymous-Id"].FirstOrDefault();
                cart = await _db.Carts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.AnonymousId == anon);
            }

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    AnonymousId = userId.HasValue ? null : anon,
                    Items = new List<CartItem>()
                };
                _db.Carts.Add(cart);
            }

            var item = cart.Items.FirstOrDefault(i => i.GameId == dto.GameId);
            if (item == null)
            {
                item = new CartItem { GameId = dto.GameId, Quantity = dto.Quantity };
                cart.Items.Add(item);
            }
            else
            {
                item.Quantity += dto.Quantity;
            }

            await _db.SaveChangesAsync();

            var totalCount = cart.Items.Sum(i => i.Quantity);

            // return updated items so client can sync immediately
            var updatedCart = await _db.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Game)
                .FirstOrDefaultAsync(c => c.Id == cart.Id);

            var items = (updatedCart?.Items ?? new List<CartItem>()).Select(i => new
            {
                gameId = i.GameId,
                quantity = i.Quantity,
                game = i.Game == null ? null : new
                {
                    id = i.Game.Id,
                    title = i.Game.Title,
                    price = i.Game.Price,
                    imageUrl = MakeAbsolute(i.Game.ImageUrl)
                }
            }).ToArray();

            // notify group
            var group = userId.HasValue ? $"user:{userId.Value}" : $"anon:{(anon ?? GetAnonymousId() ?? string.Empty)}";
            if (!string.IsNullOrEmpty(group))
            {
                await _hub.Clients.Group(group).SendAsync("CartUpdated", new { action = "add", count = totalCount });
            }

            return Ok(new { success = true, count = totalCount, items });
        }

        // DELETE: /api/cart/remove/{gameId}
        [HttpDelete("remove/{gameId}")]
        public async Task<IActionResult> RemoveFromCart(int gameId)
        {
            if (gameId <= 0) return BadRequest();

            var userId = CurrentUserId;
            var anon = Request.Headers["X-Anonymous-Id"].FirstOrDefault() ?? GetAnonymousId();

            Cart? cart = null;
            if (userId.HasValue)
            {
                cart = await _db.Carts.Include(c => c.Items)
                        .FirstOrDefaultAsync(c => c.UserId == userId.Value);
            }
            else if (!string.IsNullOrEmpty(anon))
            {
                cart = await _db.Carts.Include(c => c.Items)
                        .FirstOrDefaultAsync(c => c.AnonymousId == anon);
            }

            if (cart == null) return NotFound();

            var item = cart.Items.FirstOrDefault(i => i.GameId == gameId);
            if (item != null)
            {
                cart.Items.Remove(item);
                await _db.SaveChangesAsync();
            }

            var newCount = cart.Items.Sum(i => i.Quantity);
            var group = userId.HasValue ? $"user:{userId.Value}" : $"anon:{(anon ?? string.Empty)}";
            if (!string.IsNullOrEmpty(group))
            {
                await _hub.Clients.Group(group).SendAsync("CartUpdated", new { action = "remove", count = newCount });
            }

            return Ok();
        }

        // DELETE: /api/cart/clear
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = CurrentUserId;
            var anon = GetAnonymousId();

            Cart? cart = null;
            if (userId.HasValue)
            {
                cart = await _db.Carts.Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UserId == userId.Value);
            }
            else if (!string.IsNullOrEmpty(anon))
            {
                cart = await _db.Carts.Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.AnonymousId == anon);
            }

            if (cart == null) return Ok(new { success = true, count = 0 });

            // remove all items
            cart.Items.Clear();
            await _db.SaveChangesAsync();

            var group = userId.HasValue ? $"user:{userId.Value}" : $"anon:{(anon ?? string.Empty)}";
            if (!string.IsNullOrEmpty(group))
            {
                await _hub.Clients.Group(group).SendAsync("CartUpdated", new { action = "clear", count = 0 });
            }

            return Ok(new { success = true, count = 0 });
        }

        // POST: /api/cart/merge
        [HttpPost("merge")]
        public async Task<IActionResult> MergeCart([FromBody] string anonymousId)
        {
            // Accept the anonymous id either in the request body or from the
            // X-Anonymous-Id header (some clients send a raw string body which
            // may be delivered with a text/plain content-type). If the body is
            // empty, fall back to the header value.
            if (string.IsNullOrEmpty(anonymousId))
            {
                anonymousId = Request.Headers["X-Anonymous-Id"].FirstOrDefault() ?? string.Empty;
            }
            if (string.IsNullOrEmpty(anonymousId)) return BadRequest();
            var userId = CurrentUserId;
            if (userId == null) return Unauthorized();

            var anonCart = await _db.Carts.Include(c => c.Items)
                                .FirstOrDefaultAsync(c => c.AnonymousId == anonymousId);
            if (anonCart == null) return Ok();

            var userCart = await _db.Carts.Include(c => c.Items)
                                .FirstOrDefaultAsync(c => c.UserId == userId.Value);

            if (userCart == null)
            {
                anonCart.UserId = userId;
                anonCart.AnonymousId = null;
            }
            else
            {
                foreach (var ai in anonCart.Items.ToList())
                {
                    var existing = userCart.Items.FirstOrDefault(i => i.GameId == ai.GameId);
                    if (existing == null)
                        userCart.Items.Add(new CartItem { GameId = ai.GameId, Quantity = ai.Quantity });
                    else
                        existing.Quantity += ai.Quantity;
                }
                _db.Carts.Remove(anonCart);
            }

            await _db.SaveChangesAsync();

            // notify both groups (anon and user)
            var anonGroup = $"anon:{anonymousId}";
            var userGroup = $"user:{userId.Value}";
            var count = (userCart ?? anonCart).Items.Sum(i => i.Quantity);
            await _hub.Clients.Group(anonGroup).SendAsync("CartUpdated", new { action = "merge", count });
            await _hub.Clients.Group(userGroup).SendAsync("CartUpdated", new { action = "merge", count });

            return Ok();
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var userId = CurrentUserId;
            Cart? cart = null;
            if (userId.HasValue)
            {
                cart = await _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId.Value);
            }
            else
            {
                var anon = Request.Cookies["anonymousId"] ?? Request.Headers["X-Anonymous-Id"].FirstOrDefault();
                cart = await _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.AnonymousId == anon);
            }

            var count = cart?.Items.Sum(i => i.Quantity) ?? 0;
            return Ok(new { count });
        }
    }
}