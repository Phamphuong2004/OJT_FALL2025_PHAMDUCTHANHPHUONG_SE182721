using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using GameStoreMini.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GameStoreMini.Dtos;

namespace GameStoreMini.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public UsersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        private int? CurrentUserId
        {
            get
            {
                var sub = User?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(sub)) return null;
                if (int.TryParse(sub, out var id)) return id;
                return null;
            }
        }

        // GET: /api/users/me
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = CurrentUserId;
            if (!userId.HasValue) return Unauthorized();

            var user = _db.Users.Find(userId.Value);
            if (user == null) return NotFound();

            return Ok(new
            {
                userId = user.Id,
                email = user.Email,
                userName = user.UserName,
                fullName = user.FullName,
                phoneNumber = user.PhoneNumber,
                dateOfBirth = user.DateOfBirth,
                gender = user.Gender,
                avatar = user.Avatar,
                role = user.Role,
                emailConfirmed = user.EmailConfirmed,
                lockoutEnd = user.LockoutEnd,
                createdAt = user.CreatedAt,
                updatedAt = user.UpdatedAt
            });
        }

        // PUT: /api/users/profile
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = CurrentUserId;
            if (!userId.HasValue) return Unauthorized(new { message = "User not authenticated" });

            var user = await _db.Users.FindAsync(userId.Value);
            if (user == null) return NotFound(new { message = "User not found" });

            // Cập nhật email nếu có thay đổi
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
            {
                var emailExists = await _db.Users
                    .AnyAsync(u => u.Email == dto.Email && u.Id != user.Id);
                
                if (emailExists)
                    return BadRequest(new { message = "Email already in use" });
                
                user.Email = dto.Email;
                user.EmailConfirmed = false;
            }

            // Cập nhật các trường khác
            if (!string.IsNullOrEmpty(dto.FullName))
                user.FullName = dto.FullName;

            if (!string.IsNullOrEmpty(dto.PhoneNumber))
                user.PhoneNumber = dto.PhoneNumber;

            if (dto.DateOfBirth.HasValue)
                user.DateOfBirth = dto.DateOfBirth.Value;

            if (!string.IsNullOrEmpty(dto.Gender))
                user.Gender = dto.Gender;

            // Xử lý avatar
            if (!string.IsNullOrEmpty(dto.Avatar))
            {
                if (dto.Avatar.StartsWith("data:image"))
                {
                    try
                    {
                        var base64Data = dto.Avatar.Split(',')[1];
                        var imageBytes = Convert.FromBase64String(base64Data);
                        var fileName = $"avatar_{user.Id}_{Guid.NewGuid()}.png";
                        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "avatars");
                        
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        var filePath = Path.Combine(uploadsFolder, fileName);
                        await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
                        
                        // Xóa avatar cũ
                        if (!string.IsNullOrEmpty(user.Avatar) && user.Avatar.StartsWith("/uploads"))
                        {
                            var oldFilePath = Path.Combine(_env.WebRootPath, user.Avatar.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                                System.IO.File.Delete(oldFilePath);
                        }

                        user.Avatar = $"/uploads/avatars/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { message = "Failed to upload avatar", error = ex.Message });
                    }
                }
                else
                {
                    user.Avatar = dto.Avatar;
                }
            }

            user.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update profile", error = ex.Message });
            }

            return Ok(new
            {
                userId = user.Id,
                userName = user.UserName,
                email = user.Email,
                fullName = user.FullName,
                phoneNumber = user.PhoneNumber,
                dateOfBirth = user.DateOfBirth,
                gender = user.Gender,
                avatar = user.Avatar,
                emailConfirmed = user.EmailConfirmed,
                updatedAt = user.UpdatedAt
            });
        }

        // POST: /api/users/change-password
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = CurrentUserId;
            if (!userId.HasValue) return Unauthorized(new { message = "User not authenticated" });

            var user = await _db.Users.FindAsync(userId.Value);
            if (user == null) return NotFound(new { message = "User not found" });

            // Kiểm tra mật khẩu hiện tại
            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
                return BadRequest(new { message = "Current password is incorrect" });

            // Validate mật khẩu mới
            if (string.IsNullOrEmpty(dto.NewPassword) || dto.NewPassword.Length < 6)
                return BadRequest(new { message = "New password must be at least 6 characters long" });

            // Hash và lưu mật khẩu mới
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to change password", error = ex.Message });
            }

            return Ok(new { message = "Password changed successfully" });
        }
    }
}
