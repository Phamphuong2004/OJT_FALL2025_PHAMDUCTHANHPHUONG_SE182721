using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace GameStoreMini.Models
{
    // Simple custom user model for learning.
    public class User
    {
        public User()
        {
            Role = "Customer";
            CreatedAt = DateTime.UtcNow;
            Orders = new List<Order>();
        }

        public int Id { get; set; }

        // Unique email used to login
        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        // Username for display
        [Required, MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        // Full name / display name (optional)
        [MaxLength(200)]
        public string? FullName { get; set; }

        // Phone number (optional)
        [Phone]
        public string? PhoneNumber { get; set; }

        // Avatar image URL (stored after upload)
        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        // Store password hash (never store plain passwords)
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Role (e.g. "Customer", "Admin") - prefer roles over boolean flags
        [Required, MaxLength(50)]
        public string Role { get; set; }

        // Email confirmed flag
        public bool EmailConfirmed { get; set; } = false;

        // Optional lockout end timestamp (used by admin to lock accounts)
        public DateTime? LockoutEnd { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    // Refresh token fields for persistent login (optional)
    // The token value is stored server-side (not readable by JS when set as HttpOnly cookie)
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

        // Navigation properties
        // One user has one cart (optional)
        public Cart? Cart { get; set; }

        // One user can have many orders
        public ICollection<Order> Orders { get; set; }
    }
}