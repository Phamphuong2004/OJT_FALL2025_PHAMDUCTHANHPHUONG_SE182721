namespace Game_store.Dtos
{
    // DTOs for Admin Review Management
    public class AdminReviewsDto
    {
        public int ReviewId { get; set; }
        public int GameId { get; set; }
        public string GameTitle { get; set; } = string.Empty;
        public string? GameImageUrl { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool VerifiedPurchase { get; set; }
        public int HelpfulCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class AdminReviewListResult
    {
        public List<AdminReviewsDto> Data { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }

    public class AdminReviewsStatistics
    {
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public int VerifiedReviewsCount { get; set; }
        public double VerifiedReviewsPercentage { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
        public List<TopReviewedGame> TopReviewedGames { get; set; } = new();
    }

    public class TopReviewedGame
    {
        public int GameId { get; set; }
        public string GameTitle { get; set; } = string.Empty;
        public int ReviewCount { get; set; }
        public double AverageRating { get; set; }
    }

    public class HideReviewRequest
    {
        public bool IsHidden { get; set; }
        public string? Reason { get; set; }
    }
}