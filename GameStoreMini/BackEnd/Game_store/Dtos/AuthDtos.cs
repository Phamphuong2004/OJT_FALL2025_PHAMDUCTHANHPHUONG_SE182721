using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GameStoreMini.Dtos
{
    // DTOs (Data Transfer Objects) are used to accept and return clean data structures.
    public class RegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        // Full name shown on profile (optional)
        public string? FullName { get; set; }

        // Phone number (optional)
        public string? PhoneNumber { get; set; }

        // Avatar file (optional) - when using multipart/form-data
        public IFormFile? Avatar { get; set; }

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string ConfirmPassword { get; set; } = null!;

        // Agree to terms checkbox (should be required in UI)
        public bool TermsAccepted { get; set; } = false;
    }

    public class LoginDto
    {
        // New: accept a single identifier (email or username) for convenience
        public string? Identifier { get; set; }

        // Backwards compatibility: explicit fields still supported
        public string? Email { get; set; }
        public string? UserName { get; set; }

        [Required]
        public string Password { get; set; } = null!;
    }

    public class AuthResultDto
    {
        public string Token { get; set; } = null!;
        // Development-only: echo the refresh token in the response body to help debugging
        // (will be null in production). Remove or keep guarded by env checks.
        public string? RefreshTokenDebug { get; set; }
    }

    public class RefreshDevDto
    {
        public string? Token { get; set; }
    }
}