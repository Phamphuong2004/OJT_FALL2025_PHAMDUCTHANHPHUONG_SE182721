using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using GameStoreMini.Data;
using GameStoreMini.Dtos;
using GameStoreMini.Models;

namespace GameStoreMini.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public OrdersController(AppDbContext db) => _db = db;

        private int? CurrentUserId
        {
            get
            {
                // Thử tìm claim "sub" trước (JwtRegisteredClaimNames.Sub)
                var sub = User?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                
                // Nếu không có, thử tìm ClaimTypes.NameIdentifier
                if (string.IsNullOrEmpty(sub))
                {
                    sub = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                }
                
                if (string.IsNullOrEmpty(sub)) return null;
                if (int.TryParse(sub, out var id)) return id;
                return null;
            }
        }

        // DTO for checkout request
        public class CheckoutDto
        {
            public string? CustomerEmail { get; set; }
            public string? ShippingAddress { get; set; }
            public string? ShippingPhone { get; set; }
            public string? OrderNotes { get; set; }
        }

        // POST: /api/orders/checkout
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto? dto)
        {
            // LOG để debug
            var authHeader = Request.Headers["Authorization"].ToString();
            Console.WriteLine($"[Orders] Checkout called - Authorization header: {(!string.IsNullOrEmpty(authHeader) ? "Present (length: " + authHeader.Length + ")" : "Missing")}");
            
            // LOG TẤT CẢ CLAIMS
            Console.WriteLine($"[Orders] All claims:");
            foreach (var claim in User?.Claims ?? Enumerable.Empty<Claim>())
            {
                Console.WriteLine($"  {claim.Type} = {claim.Value}");
            }
            
            Console.WriteLine($"[Orders] CurrentUserId: {CurrentUserId?.ToString() ?? "NULL"}");
            Console.WriteLine($"[Orders] User.Identity.IsAuthenticated: {User?.Identity?.IsAuthenticated}");
            
            if (CurrentUserId == null) return Unauthorized();

            // Lấy AnonymousId từ header (frontend gửi kèm)
            var anonId = Request.Headers["X-Anonymous-Id"].ToString();
            
            Console.WriteLine($"[Orders] Looking for cart with UserId: {CurrentUserId.Value} OR AnonymousId: {anonId}");
            
            // Tìm cart theo UserId TRƯỚC
            var cart = await _db.Carts
                .Include(c => c.Items).ThenInclude(i => i.Game)
                .FirstOrDefaultAsync(c => c.UserId == CurrentUserId.Value);

            // Nếu không tìm thấy cart của user, thử tìm cart anonymous
            if ((cart == null || !cart.Items.Any()) && !string.IsNullOrEmpty(anonId))
            {
                Console.WriteLine($"[Orders] User cart not found or empty, trying anonymous cart with id: {anonId}");
                var anonymousCart = await _db.Carts
                    .Include(c => c.Items).ThenInclude(i => i.Game)
                    .FirstOrDefaultAsync(c => c.AnonymousId == anonId && c.UserId == null);
                    
                if (anonymousCart != null && anonymousCart.Items.Any())
                {
                    if (cart == null)
                    {
                        // User chưa có cart → Claim anonymous cart
                        Console.WriteLine($"[Orders] Found anonymous cart, claiming it for user {CurrentUserId.Value}");
                        anonymousCart.UserId = CurrentUserId.Value;
                        anonymousCart.AnonymousId = null;
                        cart = anonymousCart;
                    }
                    else
                    {
                        // User đã có cart → Merge items từ anonymous cart
                        Console.WriteLine($"[Orders] Merging anonymous cart items into user cart");
                        foreach (var anonItem in anonymousCart.Items)
                        {
                            var existingItem = cart.Items.FirstOrDefault(i => i.GameId == anonItem.GameId);
                            if (existingItem != null)
                            {
                                existingItem.Quantity += anonItem.Quantity;
                            }
                            else
                            {
                                cart.Items.Add(new CartItem
                                {
                                    CartId = cart.Id,
                                    GameId = anonItem.GameId,
                                    Quantity = anonItem.Quantity,
                                    UnitPrice = anonItem.UnitPrice
                                });
                            }
                        }
                        
                        // Xóa anonymous cart sau khi merge
                        _db.CartItems.RemoveRange(anonymousCart.Items);
                        _db.Carts.Remove(anonymousCart);
                    }
                    
                    await _db.SaveChangesAsync();
                }
            }

            Console.WriteLine($"[Orders] Cart found: {(cart != null ? "YES" : "NO")}");
            if (cart != null)
            {
                Console.WriteLine($"[Orders] Cart.UserId: {cart.UserId}, Cart.Items.Count: {cart.Items.Count}");
            }
            
            if (cart == null || !cart.Items.Any()) return BadRequest("Cart is empty.");

            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                // Re-check stock under transaction
                foreach (var ci in cart.Items)
                {
                    if (ci.Game == null)
                        return BadRequest("Game not found in cart.");

                    await _db.Entry(ci.Game).ReloadAsync(); // ensure fresh
                    if (ci.Game.Stock < ci.Quantity)
                        return BadRequest($"Insufficient stock for {ci.Game.Title}.");
                }

                var order = new Order 
                { 
                    UserId = CurrentUserId.Value, 
                    CreatedAt = DateTime.UtcNow,
                    CustomerEmail = dto?.CustomerEmail ?? string.Empty,
                    ShippingAddress = dto?.ShippingAddress ?? string.Empty,
                    ShippingPhone = dto?.ShippingPhone ?? string.Empty,
                    Status = "Pending",
                    PaymentStatus = "Pending"
                };
                decimal total = 0m;

                foreach (var ci in cart.Items)
                {
                    var oi = new OrderItem
                    {
                        GameId = ci.GameId,
                        Quantity = ci.Quantity,
                        UnitPrice = ci.UnitPrice // use snapshot price
                    };
                    order.Items.Add(oi);
                    total += oi.UnitPrice * oi.Quantity;

                    // decrease stock
                    ci.Game!.Stock -= ci.Quantity;
                    _db.Games.Update(ci.Game);
                }

                order.Total = total;
                _db.Orders.Add(order);

                // clear cart items
                _db.CartItems.RemoveRange(cart.Items);

                // generate a human-friendly order number
                order.OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..6]}";

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return Ok(new { order.Id, order.Total, order.CreatedAt, order.OrderNumber });
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // GET: /api/orders
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            if (CurrentUserId == null) return Unauthorized();

            var orders = await _db.Orders
                .Where(o => o.UserId == CurrentUserId.Value)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new { o.Id, o.Total, o.CreatedAt, o.Status })
                .ToListAsync();

            return Ok(orders);
        }

        // POST: /api/orders/guest-checkout
        [HttpPost("guest-checkout")]
        public async Task<IActionResult> GuestCheckout([FromBody] GuestCheckoutDto dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any()) return BadRequest();

            // Try to find anonymous cart (optional)
            Cart? cart = null;
            if (!string.IsNullOrEmpty(dto.AnonymousId))
            {
                cart = await _db.Carts.Include(c => c.Items).ThenInclude(i => i.Game)
                        .FirstOrDefaultAsync(c => c.AnonymousId == dto.AnonymousId);
            }

            // Gather field-level errors to return to client if any
            var fieldErrors = new Dictionary<string, List<string>>();

            // Validate shipping information (prefer dto.Shipping if provided)
            var ship = dto.Shipping;
            string? fullName = ship?.FullName ?? dto.CustomerName;
            string? email = ship?.FullName == null ? dto.CustomerEmail : ship?.FullName == null ? dto.CustomerEmail : (ship?.FullName == null ? dto.CustomerEmail : ship?.FullName == null ? dto.CustomerEmail : ship?.FullName == null ? dto.CustomerEmail : ship?.FullName == null ? dto.CustomerEmail : ship?.FullName);
            // Above line is intentionally left simple; prefer dto.Shipping.Email if available
            email = ship?.FullName == null ? dto.CustomerEmail : dto.CustomerEmail; // fallback logic kept simple

            if (string.IsNullOrWhiteSpace(ship?.FullName) && string.IsNullOrWhiteSpace(dto.CustomerName))
                fieldErrors.TryAdd("fullName", new List<string> { "Họ và tên bắt buộc." });

            if (string.IsNullOrWhiteSpace(ship?.Phone))
                fieldErrors.TryAdd("Phone", new List<string> { "Số điện thoại bắt buộc." });

            if (string.IsNullOrWhiteSpace(ship?.Address))
                fieldErrors.TryAdd("Address", new List<string> { "Địa chỉ bắt buộc." });

            var emailToCheck = ship?.FullName == null ? dto.CustomerEmail : ship?.FullName == null ? dto.CustomerEmail : ship?.FullName == null ? dto.CustomerEmail : dto.CustomerEmail;
            // simple email validation
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^\S+@\S+\.\S+$");
            if (string.IsNullOrWhiteSpace(dto.CustomerEmail) && string.IsNullOrWhiteSpace(ship?.FullName))
            {
                // if no email anywhere
                fieldErrors.TryAdd("email", new List<string> { "Email bắt buộc." });
            }
            else if (!string.IsNullOrWhiteSpace(dto.CustomerEmail) && !emailRegex.IsMatch(dto.CustomerEmail))
            {
                fieldErrors.TryAdd("email", new List<string> { "Email không hợp lệ." });
            }

            // Validate items and compute total, collect item-specific errors
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                decimal computedTotal = 0m;
                var orderItems = new List<OrderItem>();

                int idx = 0;
                foreach (var it in dto.Items)
                {
                    var game = await _db.Games.FindAsync(it.GameId);
                    if (game == null)
                    {
                        fieldErrors.TryAdd($"items[{idx}]", new List<string> { $"Sản phẩm {it.GameId} không tồn tại." });
                        idx++; continue;
                    }
                    if (game.Stock < it.Quantity)
                    {
                        fieldErrors.TryAdd($"items[{idx}]", new List<string> { $"Số lượng cho {game.Title} vượt quá tồn kho ({game.Stock})." });
                        idx++; continue;
                    }

                    var unitPrice = game.Price; // trust server price
                    computedTotal += unitPrice * it.Quantity;

                    orderItems.Add(new OrderItem
                    {
                        GameId = it.GameId,
                        Quantity = it.Quantity,
                        UnitPrice = unitPrice
                    });
                    idx++;
                }

                if (fieldErrors.Any())
                {
                    await tx.RollbackAsync();
                    return BadRequest(new { errors = fieldErrors });
                }

                // Build order
                var order = new Order
                {
                    UserId = CurrentUserId,
                    CustomerName = ship?.FullName ?? dto.CustomerName,
                    CustomerEmail = ship?.FullName == null ? dto.CustomerEmail : ship?.FullName == null ? dto.CustomerEmail : dto.CustomerEmail,
                    CreatedAt = DateTime.UtcNow,
                    Items = orderItems,
                    Total = computedTotal,
                    PaymentStatus = "Pending",
                    ShippingAddress = ship?.Address ?? string.Empty,
                    ShippingPhone = ship?.Phone ?? string.Empty
                };

                // store additional structured shipping fields if present
                if (ship != null)
                {
                    // attach additional fields to Order via ShippingAddress or new fields if needed
                    // For demo, append structured parts into ShippingAddress
                    order.ShippingAddress = string.Concat(ship.Address, ", ", ship.City, ", ", ship.State ?? string.Empty);
                }

                // Decrease stock for each game
                foreach (var it in dto.Items)
                {
                    var game = await _db.Games.FindAsync(it.GameId);
                    if (game == null)
                    {
                        await tx.RollbackAsync();
                        return BadRequest(new { errors = new Dictionary<string, string[]> { { "items", new[] { $"Sản phẩm {it.GameId} không tồn tại." } } } });
                    }
                    game.Stock -= it.Quantity;
                    _db.Games.Update(game);
                }

                _db.Orders.Add(order);
                if (cart != null)
                {
                    _db.Carts.Remove(cart);
                }

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return Ok(new { orderId = order.Id, order.Total });
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        [Authorize]
        [HttpGet("myorders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
            if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var orders = await _db.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items) // adjust include names
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Ok(orders);
        }

        // GET /api/orders/{id} -> get single order for customer
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
            if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null) return NotFound();
            return Ok(order);
        }

        // POST /api/orders/track  -> guest tracking by orderNumber + email
        public class TrackDto { public string OrderNumber { get; set; } = ""; public string Email { get; set; } = ""; }

        [AllowAnonymous]
        [HttpPost("track")]
        public async Task<IActionResult> TrackOrder([FromBody] TrackDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.OrderNumber) || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("OrderNumber and Email required.");

            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.OrderNumber == dto.OrderNumber && o.CustomerEmail == dto.Email);

            if (order == null) return NotFound();
            return Ok(order);
        }
    }
}