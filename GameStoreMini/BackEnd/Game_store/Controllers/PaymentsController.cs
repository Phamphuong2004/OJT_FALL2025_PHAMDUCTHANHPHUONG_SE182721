using Microsoft.AspNetCore.Mvc;
using GameStoreMini.Data;
using GameStoreMini.Dtos;
using GameStoreMini.Models;

namespace GameStoreMini.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PaymentsController(AppDbContext db) => _db = db;

        // Development-only: create a mock payment for an order
        [HttpPost("create")]
        public IActionResult Create([FromBody] CreatePaymentDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.OrderNumber)) return BadRequest();
            var order = _db.Orders.FirstOrDefault(o => o.OrderNumber == dto.OrderNumber);
            if (order == null) return NotFound();
            // Return a mock payment id/url
            var paymentId = Guid.NewGuid().ToString();
            return Ok(new { paymentId, paymentUrl = $"/mock-pay/{paymentId}" });
        }

        // Development-only: confirm a mock payment and mark order as paid
        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm([FromBody] ConfirmPaymentDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.OrderNumber)) return BadRequest();
            var order = _db.Orders.FirstOrDefault(o => o.OrderNumber == dto.OrderNumber);
            if (order == null) return NotFound();

            order.PaymentStatus = "Paid";
            order.TransactionId = dto.TransactionId ?? Guid.NewGuid().ToString();
            await _db.SaveChangesAsync();

            return Ok(new { orderId = order.Id, orderNumber = order.OrderNumber, paymentStatus = order.PaymentStatus });
        }
    }
}
