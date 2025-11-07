namespace Game_store.Dtos
{
    public class ViewHistoryDtos
    {
        public int ViewHistoryId { get; set; }
        public int GameId { get; set; }
        public string GameTitle { get; set; } = string.Empty;
        public decimal GamePrice { get; set; }
        public string? GameImageUrl { get; set; }
        public string? GameDescription { get; set; }
        public string? CategoryName { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime ViewedAt { get; set; }
        public DateTime? LastViewedAt { get; set; }
        public int ViewCount { get; set; }
    }

    public class AddViewHistoryDto
    {
        public int GameId { get; set; }
    }

    public class ViewHistoryListResult
    {
        public List<ViewHistoryDtos> Data { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}