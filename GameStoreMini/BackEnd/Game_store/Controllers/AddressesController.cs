using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameStoreMini.Data;
using Game_store.Dtos;
using Game_store.Models;
using System.Security.Claims;

namespace Game_store.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Chỉ user đã đăng nhập mới dùng được
    public class AddressesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AddressesController> _logger;

        public AddressesController(AppDbContext db, ILogger<AddressesController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // ========== HELPER METHODS ==========
        
        /// <summary>
        /// Lấy UserId từ JWT token
        /// </summary>
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            return userId;
        }

        /// <summary>
        /// Bỏ default của tất cả địa chỉ khác
        /// </summary>
        private async Task UnsetOtherDefaults(int userId)
        {
            var defaultAddresses = await _db.Addresses
                .Where(a => a.UserId == userId && a.IsDefault)
                .ToListAsync();

            foreach (var addr in defaultAddresses)
            {
                addr.IsDefault = false;
            }
        }

        // ========== API ENDPOINTS ==========

        /// <summary>
        /// GET: api/addresses
        /// Lấy tất cả địa chỉ của user hiện tại
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = GetUserId();

                var addresses = await _db.Addresses
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.IsDefault) // Default address lên đầu
                    .ThenByDescending(a => a.CreatedAt)
                    .Select(a => new AddressDto
                    {
                        Id = a.Id,
                        FullName = a.FullName,
                        PhoneNumber = a.PhoneNumber,
                        Street = a.Street,
                        Ward = a.Ward,
                        District = a.District,
                        City = a.City,
                        PostalCode = a.PostalCode,
                        IsDefault = a.IsDefault,
                        CreatedAt = a.CreatedAt,
                        UpdatedAt = a.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    data = addresses,
                    count = addresses.Count
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting addresses");
                return StatusCode(500, new { success = false, message = "Lỗi khi lấy danh sách địa chỉ" });
            }
        }

        /// <summary>
        /// GET: api/addresses/default
        /// Lấy địa chỉ mặc định
        /// </summary>
        [HttpGet("default")]
        public async Task<IActionResult> GetDefault()
        {
            try
            {
                var userId = GetUserId();

                var address = await _db.Addresses
                    .Where(a => a.UserId == userId && a.IsDefault)
                    .Select(a => new AddressDto
                    {
                        Id = a.Id,
                        FullName = a.FullName,
                        PhoneNumber = a.PhoneNumber,
                        Street = a.Street,
                        Ward = a.Ward,
                        District = a.District,
                        City = a.City,
                        PostalCode = a.PostalCode,
                        IsDefault = a.IsDefault,
                        CreatedAt = a.CreatedAt,
                        UpdatedAt = a.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (address == null)
                {
                    return NotFound(new { success = false, message = "Chưa có địa chỉ mặc định" });
                }

                return Ok(new { success = true, data = address });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting default address");
                return StatusCode(500, new { success = false, message = "Lỗi khi lấy địa chỉ mặc định" });
            }
        }

        /// <summary>
        /// GET: api/addresses/{id}
        /// Lấy chi tiết 1 địa chỉ
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var userId = GetUserId();

                var address = await _db.Addresses
                    .Where(a => a.Id == id && a.UserId == userId)
                    .Select(a => new AddressDto
                    {
                        Id = a.Id,
                        FullName = a.FullName,
                        PhoneNumber = a.PhoneNumber,
                        Street = a.Street,
                        Ward = a.Ward,
                        District = a.District,
                        City = a.City,
                        PostalCode = a.PostalCode,
                        IsDefault = a.IsDefault,
                        CreatedAt = a.CreatedAt,
                        UpdatedAt = a.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (address == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy địa chỉ" });
                }

                return Ok(new { success = true, data = address });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting address by id");
                return StatusCode(500, new { success = false, message = "Lỗi khi lấy thông tin địa chỉ" });
            }
        }

        /// <summary>
        /// POST: api/addresses
        /// Tạo địa chỉ mới
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAddressDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Dữ liệu không hợp lệ",
                        errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    });
                }

                var userId = GetUserId();

                // Nếu là địa chỉ mặc định, bỏ default của các địa chỉ khác
                if (dto.IsDefault)
                {
                    await UnsetOtherDefaults(userId);
                }

                // Nếu chưa có địa chỉ nào, tự động set làm default
                var hasAddresses = await _db.Addresses.AnyAsync(a => a.UserId == userId);
                if (!hasAddresses)
                {
                    dto.IsDefault = true;
                }

                var address = new Address
                {
                    UserId = userId,
                    FullName = dto.FullName.Trim(),
                    PhoneNumber = dto.PhoneNumber.Trim(),
                    Street = dto.Street.Trim(),
                    Ward = dto.Ward.Trim(),
                    District = dto.District.Trim(),
                    City = dto.City.Trim(),
                    PostalCode = dto.PostalCode?.Trim(),
                    IsDefault = dto.IsDefault,
                    CreatedAt = DateTime.UtcNow
                };

                _db.Addresses.Add(address);
                await _db.SaveChangesAsync();

                var result = new AddressDto
                {
                    Id = address.Id,
                    FullName = address.FullName,
                    PhoneNumber = address.PhoneNumber,
                    Street = address.Street,
                    Ward = address.Ward,
                    District = address.District,
                    City = address.City,
                    PostalCode = address.PostalCode,
                    IsDefault = address.IsDefault,
                    CreatedAt = address.CreatedAt
                };

                return CreatedAtAction(nameof(GetById), new { id = address.Id }, new
                {
                    success = true,
                    message = "Thêm địa chỉ thành công",
                    data = result
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating address");
                return StatusCode(500, new { success = false, message = "Lỗi khi tạo địa chỉ" });
            }
        }

        /// <summary>
        /// PUT: api/addresses/{id}
        /// Cập nhật địa chỉ
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAddressDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Dữ liệu không hợp lệ",
                        errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    });
                }

                var userId = GetUserId();

                var address = await _db.Addresses
                    .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

                if (address == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy địa chỉ" });
                }

                // Update thông tin
                address.FullName = dto.FullName.Trim();
                address.PhoneNumber = dto.PhoneNumber.Trim();
                address.Street = dto.Street.Trim();
                address.Ward = dto.Ward.Trim();
                address.District = dto.District.Trim();
                address.City = dto.City.Trim();
                address.PostalCode = dto.PostalCode?.Trim();
                address.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                var result = new AddressDto
                {
                    Id = address.Id,
                    FullName = address.FullName,
                    PhoneNumber = address.PhoneNumber,
                    Street = address.Street,
                    Ward = address.Ward,
                    District = address.District,
                    City = address.City,
                    PostalCode = address.PostalCode,
                    IsDefault = address.IsDefault,
                    CreatedAt = address.CreatedAt,
                    UpdatedAt = address.UpdatedAt
                };

                return Ok(new
                {
                    success = true,
                    message = "Cập nhật địa chỉ thành công",
                    data = result
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating address");
                return StatusCode(500, new { success = false, message = "Lỗi khi cập nhật địa chỉ" });
            }
        }

        /// <summary>
        /// PUT: api/addresses/{id}/set-default
        /// Đặt địa chỉ làm mặc định
        /// </summary>
        [HttpPut("{id}/set-default")]
        public async Task<IActionResult> SetDefault(int id)
        {
            try
            {
                var userId = GetUserId();

                var address = await _db.Addresses
                    .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

                if (address == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy địa chỉ" });
                }

                // Bỏ default của các địa chỉ khác
                await UnsetOtherDefaults(userId);

                // Set địa chỉ này làm default
                address.IsDefault = true;
                address.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Đã đặt làm địa chỉ mặc định"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting default address");
                return StatusCode(500, new { success = false, message = "Lỗi khi đặt địa chỉ mặc định" });
            }
        }

        /// <summary>
        /// DELETE: api/addresses/{id}
        /// Xóa địa chỉ
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = GetUserId();

                var address = await _db.Addresses
                    .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

                if (address == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy địa chỉ" });
                }

                var wasDefault = address.IsDefault;

                _db.Addresses.Remove(address);
                await _db.SaveChangesAsync();

                // Nếu xóa địa chỉ default, tự động set địa chỉ khác làm default
                if (wasDefault)
                {
                    var newDefault = await _db.Addresses
                        .Where(a => a.UserId == userId)
                        .OrderByDescending(a => a.CreatedAt)
                        .FirstOrDefaultAsync();

                    if (newDefault != null)
                    {
                        newDefault.IsDefault = true;
                        await _db.SaveChangesAsync();
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = "Xóa địa chỉ thành công"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting address");
                return StatusCode(500, new { success = false, message = "Lỗi khi xóa địa chỉ" });
            }
        }
    }
}
