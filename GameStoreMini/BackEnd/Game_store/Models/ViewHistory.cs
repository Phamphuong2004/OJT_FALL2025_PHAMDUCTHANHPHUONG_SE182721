using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStoreMini.Models;

namespace Game_store.Models
{
    public class ViewHistory
    {
[Key]
        public int ViewHistoryId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int GameId { get; set; }

        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;

        public int ViewCount { get; set; } = 1; // Số lần xem game này

        public DateTime? LastViewedAt { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("GameId")]
        public virtual Game Game { get; set; } = null!;
    }
}