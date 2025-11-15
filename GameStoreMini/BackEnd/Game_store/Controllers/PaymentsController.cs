using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameStoreMini.Data;
using Game_store.Services;
using GameStoreMini.Dtos;

namespace Game_store.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly VnPayService _vnPayService;
        private readonly AppDbContext _db;

        public PaymentsController(VnPayService vnPayService, AppDbContext db)
        {
            _vnPayService = vnPayService;
            _db = db;
        }

        [HttpPost("create-payment-url")]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] CreatePaymentRequest request)
        {
            try
            {
                var order = await _db.Orders
                    .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber);

                if (order == null)
                    return NotFound(new { message = "Không tìm thấy đơn hàng" });

                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (ipAddress == "::1") ipAddress = "127.0.0.1";
                ipAddress ??= "127.0.0.1";
                
                var orderInfo = $"Order {order.OrderNumber}";

                var paymentUrl = _vnPayService.CreatePaymentUrl(
                    order.OrderNumber!,
                    order.Total,
                    orderInfo,
                    ipAddress
                );

                return Ok(new { paymentUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn()
        {
            Console.WriteLine("🔔 VnPayReturn called!");
            Console.WriteLine($"🔔 Query string: {Request.QueryString}");
            
            try
            {
                var vnpParams = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());
                Console.WriteLine($"🔔 Params count: {vnpParams.Count}");
                
                if (!vnpParams.ContainsKey("vnp_SecureHash"))
                {
                    Console.WriteLine("❌ Missing vnp_SecureHash!");
                    return Redirect("http://localhost:5173/payment/result?success=false&message=Missing signature");
                }

                var vnp_SecureHash = vnpParams["vnp_SecureHash"];

                if (!_vnPayService.ValidateSignature(vnpParams, vnp_SecureHash))
                {
                    return Redirect("http://localhost:5173/payment/result?success=false&message=Invalid signature");
                }

                var vnp_ResponseCode = vnpParams["vnp_ResponseCode"];
                var vnp_TxnRef = vnpParams["vnp_TxnRef"];
                var vnp_TransactionNo = vnpParams.ContainsKey("vnp_TransactionNo") 
                    ? vnpParams["vnp_TransactionNo"] 
                    : null;

                var order = await _db.Orders.FirstOrDefaultAsync(o => o.OrderNumber == vnp_TxnRef);
                
                if (order == null)
                {
                    return Redirect("http://localhost:5173/payment/result?success=false&message=Order not found");
                }

                if (vnp_ResponseCode == "00")
                {
                    order.PaymentStatus = "Paid";
                    order.Status = "Confirmed";
                    order.TransactionId = vnp_TransactionNo;
                    await _db.SaveChangesAsync();

                    return Redirect($"http://localhost:5173/payment/result?success=true&orderNumber={order.OrderNumber}");
                }
                else
                {
                    order.PaymentStatus = "Failed";
                    await _db.SaveChangesAsync();

                    return Redirect($"http://localhost:5173/payment/result?success=false&message=Payment failed&code={vnp_ResponseCode}");
                }
            }
            catch (Exception ex)
            {
                return Redirect($"http://localhost:5173/payment/result?success=false&message={ex.Message}");
            }
        }
    }
}
