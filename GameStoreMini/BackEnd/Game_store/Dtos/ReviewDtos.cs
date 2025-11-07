using System.ComponentModel.DataAnnotations;

namespace Game_store.Dtos
{
    // DTO để tạo review mới
    public class CreateReviewDto
    {
        [Required]
        public int GameId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating phải từ 1 đến 5")]
        public int Rating { get; set; }

        [MaxLength(1000, ErrorMessage = "Comment không được quá 1000 ký tự")]
        public string? Comment { get; set; }
    }

    // DTO để update review
    public class UpdateReviewDto
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Rating phải từ 1 đến 5")]
        public int Rating { get; set; }

        [MaxLength(1000, ErrorMessage = "Comment không được quá 1000 ký tự")]
        public string? Comment { get; set; }
    }

    // DTO để trả về review
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public int GameId { get; set; }
        public string? GameTitle { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsVerifiedPurchase { get; set; }
        public int HelpfulCount { get; set; }
        public bool? IsHelpfulByCurrentUser { get; set; } // User hiện tại đã vote helpful chưa
    }

    // DTO để lọc reviews
    public class ReviewFilterDto
    {
        public int? GameId { get; set; }
        public int? UserId { get; set; }
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
        public bool? VerifiedPurchaseOnly { get; set; }
        public string? SortBy { get; set; } = "date"; // date, rating, helpful
        public string? SortOrder { get; set; } = "desc"; // asc, desc
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    // DTO cho thống kê rating
    public class RatingStatisticsDto
    {
        public int GameId { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new(); // Star: Count
    }
}