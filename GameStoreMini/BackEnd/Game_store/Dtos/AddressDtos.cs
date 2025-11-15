using System.ComponentModel.DataAnnotations;

namespace Game_store.Dtos
{
    // DTO để trả về client
    public class AddressDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Địa chỉ đầy đủ để hiển thị
        public string FullAddress => $"{Street}, {Ward}, {District}, {City}";
    }

    // DTO để tạo địa chỉ mới
    public class CreateAddressDto
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không được quá 100 ký tự")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [RegularExpression(@"^(0|\+84)[0-9]{9,10}$", ErrorMessage = "Số điện thoại phải là số Việt Nam hợp lệ (bắt đầu bằng 0 hoặc +84)")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ đường/số nhà là bắt buộc")]
        [StringLength(200, ErrorMessage = "Địa chỉ không được quá 200 ký tự")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phường/Xã là bắt buộc")]
        [StringLength(100, ErrorMessage = "Phường/Xã không được quá 100 ký tự")]
        public string Ward { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quận/Huyện là bắt buộc")]
        [StringLength(100, ErrorMessage = "Quận/Huyện không được quá 100 ký tự")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tỉnh/Thành phố là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tỉnh/Thành phố không được quá 100 ký tự")]
        public string City { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "Mã bưu điện không được quá 10 ký tự")]
        public string? PostalCode { get; set; }

        public bool IsDefault { get; set; } = false;
    }

    // DTO để cập nhật địa chỉ
    public class UpdateAddressDto
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không được quá 100 ký tự")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [RegularExpression(@"^(0|\+84)[0-9]{9,10}$", ErrorMessage = "Số điện thoại phải là số Việt Nam hợp lệ")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ đường/số nhà là bắt buộc")]
        [StringLength(200, ErrorMessage = "Địa chỉ không được quá 200 ký tự")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phường/Xã là bắt buộc")]
        [StringLength(100, ErrorMessage = "Phường/Xã không được quá 100 ký tự")]
        public string Ward { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quận/Huyện là bắt buộc")]
        [StringLength(100, ErrorMessage = "Quận/Huyện không được quá 100 ký tự")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tỉnh/Thành phố là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tỉnh/Thành phố không được quá 100 ký tự")]
        public string City { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "Mã bưu điện không được quá 10 ký tự")]
        public string? PostalCode { get; set; }
    }
}
