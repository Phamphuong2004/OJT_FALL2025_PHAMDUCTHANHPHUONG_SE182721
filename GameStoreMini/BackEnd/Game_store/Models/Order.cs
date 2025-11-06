namespace GameStoreMini.Models
{
    public class Order
    {
        public int Id { get; set; }
        // nullable so guest orders are supported
        public int? UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        // Simple status string for demo
        public string Status { get; set; } = "Created";
        // Guest info (optional)
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
    // Human-friendly order number for tracking (e.g. ORD-20251106...)
    public string? OrderNumber { get; set; }
        
        // Payment and shipping metadata
        public string? PaymentMethod { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string? TransactionId { get; set; }
        // Optional: store shipping address/phone as a single field or structured fields
        public string? ShippingAddress { get; set; }
        public string? ShippingPhone { get; set; }
    }
}