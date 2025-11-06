namespace GameStoreMini.Dtos
{
    public class AddToCartDto
    {
        public int GameId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class CartItemDto
    {
        public int GameId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class CartDto
    {
        public IEnumerable<CartItemDto> Items { get; set; } = Array.Empty<CartItemDto>();
        public decimal Total { get; set; }
    }
}