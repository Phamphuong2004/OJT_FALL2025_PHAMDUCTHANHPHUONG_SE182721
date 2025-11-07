using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStoreMini.Models;

namespace Game_store.Models
{
    public class ReviewGame
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        public int GameId { get; set; }

        [ForeignKey("GameId")]
        public Game? Game { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // 1-5 sao

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsVerifiedPurchase { get; set; } // Đã mua game chưa

        // Thống kê
        public int HelpfulCount { get; set; } = 0; // Số người thấy hữu ích
    }
}
