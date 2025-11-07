namespace Game_store.Dtos
{
    public class WishlistItemDto
    {
        public int WishlistId { get; set; }
        public int GameId { get; set; }
        public string GameTitle { get; set; } = string.Empty;
        public decimal GamePrice { get; set; }
        public string? GameImageUrl { get; set; }
        public string? GameDescription { get; set; }
        public string? CategoryName { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
