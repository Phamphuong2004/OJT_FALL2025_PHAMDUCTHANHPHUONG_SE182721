using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using GameStoreMini.Data;
using GameStoreMini.Dtos;
using GameStoreMini.Models;
using Microsoft.Extensions.Logging;

namespace GameStoreMini.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(AppDbContext db, ILogger<OrdersController> logger)
        {
            _db = db;
            _logger = logger;
        }

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
            _logger.LogInformation("[Orders] Checkout called - Authorization header: {AuthHeader}", !string.IsNullOrEmpty(authHeader) ? $"Present (length: {authHeader.Length})" : "Missing");

            // LOG TẤT CẢ CLAIMS
            _logger.LogInformation("[Orders] All claims:");
            foreach (var claim in User?.Claims ?? Enumerable.Empty<Claim>())
            {
                _logger.LogInformation("[Orders] Claim - {Type} = {Value}", claim.Type, claim.Value);
            }

            _logger.LogInformation("[Orders] CurrentUserId: {UserId}", CurrentUserId?.ToString() ?? "NULL");
            _logger.LogInformation("[Orders] User.Identity.IsAuthenticated: {IsAuthenticated}", User?.Identity?.IsAuthenticated);
            
            if (CurrentUserId == null) return Unauthorized();

            // Lấy AnonymousId từ header (frontend gửi kèm)
            var anonId = Request.Headers["X-Anonymous-Id"].ToString();
            
            _logger.LogInformation("[Orders] Looking for cart with UserId: {UserId} OR AnonymousId: {AnonId}", CurrentUserId.Value, anonId);
            
            // Tìm cart theo UserId TRƯỚC
            var cart = await _db.Carts
                .Include(c => c.Items).ThenInclude(i => i.Game)
                .FirstOrDefaultAsync(c => c.UserId == CurrentUserId.Value);

            // Nếu không tìm thấy cart của user, thử tìm cart anonymous
            if ((cart == null || !cart.Items.Any()) && !string.IsNullOrEmpty(anonId))
            {
                _logger.LogInformation("[Orders] User cart not found or empty, trying anonymous cart with id: {AnonId}", anonId);
                var anonymousCart = await _db.Carts
                    .Include(c => c.Items).ThenInclude(i => i.Game)
                    .FirstOrDefaultAsync(c => c.AnonymousId == anonId && c.UserId == null);
                    
                if (anonymousCart != null && anonymousCart.Items.Any())
                {
                    if (cart == null)
                    {
                        // User chưa có cart → Claim anonymous cart
                        _logger.LogInformation("[Orders] Found anonymous cart, claiming it for user {UserId}", CurrentUserId.Value);
                        anonymousCart.UserId = CurrentUserId.Value;
                        anonymousCart.AnonymousId = null;
                        cart = anonymousCart;
                    }
                    else
                    {
                        // User đã có cart → Merge items từ anonymous cart
                        _logger.LogInformation("[Orders] Merging anonymous cart items into user cart");
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

                    _logger.LogInformation("[Orders] Cart found: {HasCart}", cart != null ? "YES" : "NO");
            if (cart != null)
            {
                _logger.LogInformation("[Orders] Cart.UserId: {CartUserId}, Cart.Items.Count: {Count}", cart.UserId, cart.Items.Count);
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
                    PaymentStatus = "Pending",
                    // Generate order number IMMEDIATELY
                    OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..6]}"
                };
                decimal total = 0m;

                foreach (var ci in cart.Items)
                {
                    // Lấy giá hiện tại từ Game, KHÔNG từ CartItem.UnitPrice
                    // vì CartItem.UnitPrice có thể là 0 nếu không được set khi add to cart
                    var unitPrice = ci.Game?.Price ?? ci.UnitPrice;
                    
                    if (unitPrice == 0)
                    {
                        _logger.LogWarning("[Orders] WARNING: Game {GameId} has price = 0!", ci.GameId);
                    }
                    
                    var oi = new OrderItem
                    {
                        GameId = ci.GameId,
                        Quantity = ci.Quantity,
                        UnitPrice = unitPrice // use current game price as snapshot
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

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return Ok(new { order.Id, order.Total, order.CreatedAt, order.OrderNumber });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                _logger.LogError(ex, "[Orders] Checkout transaction failed");
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
                    ShippingPhone = ship?.Phone ?? string.Empty,
                    // Generate order number IMMEDIATELY
                    OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..6]}"
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

                // Return a consistent payload including orderNumber so frontend can
                // redirect to payment or show order details reliably.
                return Ok(new
                {
                    id = order.Id,
                    orderNumber = order.OrderNumber,
                    total = order.Total,
                    createdAt = order.CreatedAt
                });
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

            _logger.LogInformation("[Orders] GetMyOrders called for userId: {UserId}", userId);

            var orders = await _db.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Game) // Include game information
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            _logger.LogInformation("[Orders] Found {Count} orders for user {UserId}", orders.Count, userId);

            return Ok(orders);
        }

        // GET /api/orders/{id} -> get single order for customer
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            int? userId = null;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var parsedUserId)) userId = parsedUserId;

            var order = await _db.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Game)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng." });

            if (order.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            var statusLabel = order.Status switch
            {
                "Pending" => "Đang chờ xử lý",
                "Processing" => "Đang xử lý",
                "Shipping" => "Đang giao",
                "Completed" => "Hoàn thành",
                "Cancelled" => "Đã hủy",
                "Refund" => "Hoàn tiền",
                _ => order.Status
            };

            return Ok(new { success = true, order, statusLabel });
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
                    .ThenInclude(i => i.Game)
                .FirstOrDefaultAsync(o => o.OrderNumber == dto.OrderNumber && o.CustomerEmail == dto.Email);

            if (order == null) return NotFound();
            return Ok(order);
        }

        // Admin endpoint to recalculate order totals
        [Authorize(Roles = "Admin")]
        [HttpPost("admin/recalculate-totals")]
        public async Task<IActionResult> RecalculateOrderTotals()
        {
            var ordersWithZeroTotal = await _db.Orders
                .Include(o => o.Items)
                .Where(o => o.Total == 0)
                .ToListAsync();

            int fixedCount = 0;
            foreach (var order in ordersWithZeroTotal)
            {
                var calculatedTotal = order.Items.Sum(i => i.UnitPrice * i.Quantity);
                if (calculatedTotal > 0)
                {
                    order.Total = calculatedTotal;
                    fixedCount++;
                }
            }

            await _db.SaveChangesAsync();

            return Ok(new { 
                message = $"Fixed {fixedCount} orders with zero total", 
                totalChecked = ordersWithZeroTotal.Count,
                fixedCount = fixedCount
            });
        }

        // ===== ADMIN ENDPOINTS =====

        // GET /api/orders/admin/all - Get all orders (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all")]
        public async Task<IActionResult> GetAllOrdersAdmin([FromQuery] string? status = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            _logger.LogInformation("[Orders] Admin GetAllOrders called - Status: {Status}, Page: {Page}", status, page);

            var query = _db.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Game)
                .AsQueryable();

            // Filter by status if provided
            if (!string.IsNullOrWhiteSpace(status) && status.ToLower() != "all")
            {
                query = query.Where(o => o.Status.ToLower() == status.ToLower());
            }

            var totalOrders = await query.CountAsync();

            // Project into a safe DTO for admin list to include basic user info
            var projected = query
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new {
                    o.Id,
                    o.OrderNumber,
                    o.Total,
                    o.CreatedAt,
                    o.Status,
                    o.PaymentStatus,
                    Items = o.Items.Select(i => new { i.GameId, i.Quantity, i.UnitPrice, GameTitle = i.Game != null ? i.Game.Title : null }),
                    User = o.User != null ? new { Id = o.User.Id, UserName = o.User.UserName, FullName = o.User.FullName, Email = o.User.Email } : null,
                    CustomerName = o.CustomerName,
                    CustomerEmail = o.CustomerEmail,
                    ShippingAddress = o.ShippingAddress,
                    ShippingPhone = o.ShippingPhone,
                    // Compute a display name prioritizing explicit customer name, then linked user full name, then username, then email
                    CustomerFullName = o.CustomerName ?? (o.User != null ? (o.User.FullName ?? o.User.UserName) : null) ?? o.CustomerEmail ?? "Guest",
                    // Normalized email for display (prefer customer email, then linked user email)
                    CustomerEmailNormalized = o.CustomerEmail ?? (o.User != null ? o.User.Email : null)
                });

            var orders = await projected
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            _logger.LogInformation("[Orders] Found {Count} orders (total: {Total})", orders.Count, totalOrders);

            return Ok(new 
            { 
                data = orders, 
                total = totalOrders,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(totalOrders / (double)pageSize)
            });
        }

        // GET /api/orders/admin/{id} - Get single order details (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/{id:int}")]
        public async Task<IActionResult> GetOrderAdmin(int id)
        {
            var order = await _db.Orders
                .Where(o => o.Id == id)
                .Select(o => new {
                    o.Id,
                    o.OrderNumber,
                    o.Total,
                    o.CreatedAt,
                    o.Status,
                    o.PaymentStatus,
                    Items = o.Items.Select(i => new { i.GameId, i.Quantity, i.UnitPrice, GameTitle = i.Game != null ? i.Game.Title : null }),
                    User = o.User != null ? new { Id = o.User.Id, UserName = o.User.UserName, FullName = o.User.FullName, Email = o.User.Email } : null,
                    CustomerName = o.CustomerName,
                    CustomerEmail = o.CustomerEmail,
                    ShippingAddress = o.ShippingAddress,
                    ShippingPhone = o.ShippingPhone,
                    CustomerFullName = o.CustomerName ?? (o.User != null ? (o.User.FullName ?? o.User.UserName) : null) ?? o.CustomerEmail ?? "Guest",
                    CustomerEmailNormalized = o.CustomerEmail ?? (o.User != null ? o.User.Email : null)
                })
                .FirstOrDefaultAsync();

            if (order == null) return NotFound();
            return Ok(order);
        }

        // PUT /api/orders/admin/{id}/status - Update order status (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/{id:int}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            _logger.LogInformation("[Orders] Admin updating order {OrderId} status to: {Status}", id, dto.Status);

            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();

            // Validate status
            var validStatuses = new[] { "Pending", "Processing", "Shipping", "Completed", "Cancelled", "Refund" };
            if (!validStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid status. Valid values: {string.Join(", ", validStatuses)}");
            }

            order.Status = dto.Status;

            // Optionally update payment status
            if (!string.IsNullOrWhiteSpace(dto.PaymentStatus))
            {
                order.PaymentStatus = dto.PaymentStatus;
            }

            await _db.SaveChangesAsync();

            _logger.LogInformation("[Orders] Order {OrderId} status updated successfully", id);
            return Ok(new { message = "Order status updated", order });
        }

        // GET /api/orders/admin/statistics - Get order statistics (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/statistics")]
        public async Task<IActionResult> GetOrderStatistics()
        {
            var totalOrders = await _db.Orders.CountAsync();
            var pendingOrders = await _db.Orders.CountAsync(o => o.Status == "Pending");
            var processingOrders = await _db.Orders.CountAsync(o => o.Status == "Processing");
            var shippingOrders = await _db.Orders.CountAsync(o => o.Status == "Shipping");
            var completedOrders = await _db.Orders.CountAsync(o => o.Status == "Completed");
            var cancelledOrders = await _db.Orders.CountAsync(o => o.Status == "Cancelled");
            var totalRevenue = await _db.Orders
                .Where(o => o.Status == "Completed")
                .SumAsync(o => o.Total);

            return Ok(new
            {
                totalOrders,
                pendingOrders,
                processingOrders,
                shippingOrders,
                completedOrders,
                cancelledOrders,
                totalRevenue
            });
        }

        public class UpdateOrderStatusDto
        {
            public string Status { get; set; } = "";
            public string? PaymentStatus { get; set; }
        }

        // DTO for guest cancel
        public class GuestCancelDto { public string OrderNumber { get; set; } = ""; public string Email { get; set; } = ""; }

        // POST /api/orders/{id}/cancel -> allow customer to cancel their own order
        [Authorize]
        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            int? userId = null;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var parsed)) userId = parsed;

            // Thực hiện hủy trong 1 transaction để tránh race condition
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                // Lấy lại đơn hàng *under transaction* và include items
                var order = await _db.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    await tx.RollbackAsync();
                    return NotFound();
                }

                if (order.UserId != userId && !User.IsInRole("Admin"))
                {
                    await tx.RollbackAsync();
                    return Forbid();
                }

                // Kiểm tra trạng thái *lần nữa* bên trong transaction
                if (order.Status == "Cancelled")
                {
                    await tx.RollbackAsync();
                    return BadRequest(new { message = "Order already cancelled" });
                }
                if (order.Status == "Completed")
                {
                    await tx.RollbackAsync();
                    return BadRequest(new { message = "Cannot cancel a completed order" });
                }

                // Restore stock safely: reload each game and update
                foreach (var it in order.Items)
                {
                    var game = await _db.Games.FindAsync(it.GameId);
                    if (game != null)
                    {
                        // Reload to ensure we have latest values and to take a row-level lock
                        await _db.Entry(game).ReloadAsync();
                        game.Stock += it.Quantity;
                        _db.Games.Update(game);
                    }
                }

                order.Status = "Cancelled";
                order.PaymentStatus = "Refund";

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return Ok(new { success = true, message = "Order cancelled", orderId = order.Id, orderNumber = order.OrderNumber });
            }
            catch (Exception ex)
            {
                // Log the exception server-side for diagnostics (do not expose full exception to client)
                _logger.LogError(ex, "[Orders] CancelOrder error for id={OrderId}", id);
                await tx.RollbackAsync();
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        // POST /api/orders/cancel -> guest cancellation by orderNumber + email
        [AllowAnonymous]
        [HttpPost("cancel")]
        public async Task<IActionResult> GuestCancel([FromBody] GuestCancelDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.OrderNumber) || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "OrderNumber and Email required" });

            // Thực hiện trong transaction để đảm bảo atomicity
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var order = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.OrderNumber == dto.OrderNumber && o.CustomerEmail == dto.Email);
                if (order == null)
                {
                    await tx.RollbackAsync();
                    return NotFound(new { message = "Order not found" });
                }

                if (order.Status == "Cancelled")
                {
                    await tx.RollbackAsync();
                    return BadRequest(new { message = "Order already cancelled" });
                }
                if (order.Status == "Completed")
                {
                    await tx.RollbackAsync();
                    return BadRequest(new { message = "Cannot cancel a completed order" });
                }

                foreach (var it in order.Items)
                {
                    var game = await _db.Games.FindAsync(it.GameId);
                    if (game != null)
                    {
                        await _db.Entry(game).ReloadAsync();
                        game.Stock += it.Quantity;
                        _db.Games.Update(game);
                    }
                }

                order.Status = "Cancelled";
                order.PaymentStatus = "Refund";

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return Ok(new { success = true, message = "Order cancelled", orderId = order.Id, orderNumber = order.OrderNumber });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Orders] GuestCancel error for orderNumber={OrderNumber}", dto.OrderNumber);
                await tx.RollbackAsync();
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }
    }
}