using System.ComponentModel.DataAnnotations;
using GameStoreMini.Models;

namespace Game_store.Models
{
    public class Promotion
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Summary { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        [MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; }

        public decimal? FixedDiscountAmount { get; set; }

        [MaxLength(50)]
        public string PromotionType { get; set; } = string.Empty; // "PERCENTAGE", "FIXED", "SPECIAL"

        [MaxLength(100)]
        public string EventType { get; set; } = string.Empty; // "BLACK_FRIDAY", "TET", "SUMMER", etc.

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        // Navigation properties
        public virtual ICollection<PromotionGame> PromotionGames { get; set; } = new List<PromotionGame>();
        // Claims by users
        public virtual ICollection<PromotionClaim> PromotionClaims { get; set; } = new List<PromotionClaim>();
    }

    public class PromotionGame
    {
        public int Id { get; set; }
        public int PromotionId { get; set; }
        public int GameId { get; set; }

        public virtual Promotion Promotion { get; set; } = null!;
        public virtual Game Game { get; set; } = null!;
    }
}