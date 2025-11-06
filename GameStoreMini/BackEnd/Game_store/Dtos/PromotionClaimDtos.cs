using System.ComponentModel.DataAnnotations;

namespace Game_store.Dtos
{
    public class CreatePromotionClaimDto
    {
        [Required]
        public int PromotionId { get; set; }
    }

    public class PromotionClaimDto
    {
        public int Id { get; set; }
        public int PromotionId { get; set; }
        public int UserId { get; set; }
        public DateTime ClaimedAt { get; set; }
        public bool IsRedeemed { get; set; }
        public DateTime? RedeemedAt { get; set; }
        public string? Notes { get; set; }
        // optional: include promotion summary for quick UI
        public string? PromotionTitle { get; set; }
        public string? PromotionImageUrl { get; set; }
    }
}
