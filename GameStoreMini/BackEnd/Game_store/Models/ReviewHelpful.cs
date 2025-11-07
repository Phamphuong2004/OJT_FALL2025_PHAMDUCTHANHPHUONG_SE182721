using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStoreMini.Models;

namespace Game_store.Models
{
    public class ReviewHelpful
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReviewId { get; set; }

        [ForeignKey("ReviewId")]
        public ReviewGame? Review { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        [Required]
        public bool Helpful { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}