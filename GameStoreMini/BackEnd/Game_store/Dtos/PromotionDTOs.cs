using System.ComponentModel.DataAnnotations;

namespace Game_store.Dtos
{
    public class CreatePromotionDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Summary { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }

        [MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; }

        public decimal? FixedDiscountAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string PromotionType { get; set; } = "PERCENTAGE";

        [Required]
        [MaxLength(100)]
        public string EventType { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        public List<int> GameIds { get; set; } = new List<int>();
    }

    public class UpdatePromotionDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Summary { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }

        [MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; }

        public decimal? FixedDiscountAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string PromotionType { get; set; } = "PERCENTAGE";

        [Required]
        [MaxLength(100)]
        public string EventType { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        public List<int> GameIds { get; set; } = new List<int>();
    }

    public class PromotionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string Slug { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal? FixedDiscountAmount { get; set; }
        public string PromotionType { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public List<GameStoreMini.Dtos.GameDto> Games { get; set; } = new List<GameStoreMini.Dtos.GameDto>();
    }

    public class PromotionStatsDto
    {
        public int TotalPromotions { get; set; }
        public int ActivePromotions { get; set; }
        public int ExpiredPromotions { get; set; }
        public int FeaturedPromotions { get; set; }
        public decimal AverageDiscountPercentage { get; set; }
        public int TotalGamesInPromotions { get; set; }
    }
}