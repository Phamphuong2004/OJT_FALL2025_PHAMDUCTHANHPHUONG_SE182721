using System.ComponentModel.DataAnnotations;
using GameStoreMini.Models;

namespace Game_store.Models
{
    public class PromotionClaim
    {
        public int Id { get; set; }

        [Required]
        public int PromotionId { get; set; }

        // Link to User (nullable only if you want to support anonymous claims).
        public int UserId { get; set; }

        public DateTime ClaimedAt { get; set; } = DateTime.UtcNow;

        public bool IsRedeemed { get; set; } = false;

        public DateTime? RedeemedAt { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Navigation
        public virtual Promotion Promotion { get; set; } = null!;
        public virtual GameStoreMini.Models.User User { get; set; } = null!;
    }
}
