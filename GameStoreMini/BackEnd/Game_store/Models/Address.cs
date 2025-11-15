using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStoreMini.Models;

namespace Game_store.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Street { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Ward { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string District { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [StringLength(10)]
        public string? PostalCode { get; set; }

        public bool IsDefault { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}