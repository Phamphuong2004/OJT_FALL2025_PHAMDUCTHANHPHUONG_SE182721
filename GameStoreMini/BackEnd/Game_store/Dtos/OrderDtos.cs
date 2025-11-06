namespace GameStoreMini.Dtos
{
    public class GuestCheckoutItemDto
    {
        public int GameId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class GuestCheckoutDto
    {
        public string? AnonymousId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        // Optional structured shipping object (frontend can send this)
        public ShippingDto? Shipping { get; set; }
        public IEnumerable<GuestCheckoutItemDto>? Items { get; set; }
        public decimal Total { get; set; }
    }
}

public class ShippingDto
{
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostCode { get; set; }
    public string? Country { get; set; }
    public string? Note { get; set; }
}
