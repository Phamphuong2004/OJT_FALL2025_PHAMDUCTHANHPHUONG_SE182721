using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class SeedLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm data Locations - Đầy đủ 63 tỉnh thành Việt Nam
            migrationBuilder.Sql(@"
                INSERT INTO ""Locations"" (""Country"", ""City"", ""District"", ""Code"") VALUES
                -- Miền Bắc
                ('Việt Nam', 'Hà Nội', 'Thành phố', '01'),
                ('Việt Nam', 'Hà Giang', 'Thành phố Hà Giang', '02'),
                ('Việt Nam', 'Cao Bằng', 'Thành phố Cao Bằng', '04'),
                ('Việt Nam', 'Bắc Kạn', 'Thành phố Bắc Kạn', '06'),
                ('Việt Nam', 'Tuyên Quang', 'Thành phố Tuyên Quang', '08'),
                ('Việt Nam', 'Lào Cai', 'Thành phố Lào Cai', '10'),
                ('Việt Nam', 'Điện Biên', 'Thành phố Điện Biên Phủ', '11'),
                ('Việt Nam', 'Lai Châu', 'Thành phố Lai Châu', '12'),
                ('Việt Nam', 'Sơn La', 'Thành phố Sơn La', '14'),
                ('Việt Nam', 'Yên Bái', 'Thành phố Yên Bái', '15'),
                ('Việt Nam', 'Hòa Bình', 'Thành phố Hòa Bình', '17'),
                ('Việt Nam', 'Thái Nguyên', 'Thành phố Thái Nguyên', '19'),
                ('Việt Nam', 'Lạng Sơn', 'Thành phố Lạng Sơn', '20'),
                ('Việt Nam', 'Quảng Ninh', 'Thành phố Hạ Long', '22'),
                ('Việt Nam', 'Bắc Giang', 'Thành phố Bắc Giang', '24'),
                ('Việt Nam', 'Phú Thọ', 'Thành phố Việt Trì', '25'),
                ('Việt Nam', 'Vĩnh Phúc', 'Thành phố Vĩnh Yên', '26'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', '27'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', '30'),
                ('Việt Nam', 'Hải Phòng', 'Thành phố', '31'),
                ('Việt Nam', 'Hưng Yên', 'Thành phố Hưng Yên', '33'),
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', '34'),
                ('Việt Nam', 'Hà Nam', 'Thành phố Phủ Lý', '35'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', '36'),
                ('Việt Nam', 'Ninh Bình', 'Thành phố Ninh Bình', '37'),
                
                -- Miền Trung
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', '38'),
                ('Việt Nam', 'Nghệ An', 'Thành phố Vinh', '40'),
                ('Việt Nam', 'Hà Tĩnh', 'Thành phố Hà Tĩnh', '42'),
                ('Việt Nam', 'Quảng Bình', 'Thành phố Đồng Hới', '44'),
                ('Việt Nam', 'Quảng Trị', 'Thành phố Đông Hà', '45'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Thành phố Huế', '46'),
                ('Việt Nam', 'Đà Nẵng', 'Thành phố', '48'),
                ('Việt Nam', 'Quảng Nam', 'Thành phố Tam Kỳ', '49'),
                ('Việt Nam', 'Quảng Ngãi', 'Thành phố Quảng Ngãi', '51'),
                ('Việt Nam', 'Bình Định', 'Thành phố Quy Nhơn', '52'),
                ('Việt Nam', 'Phú Yên', 'Thành phố Tuy Hòa', '54'),
                ('Việt Nam', 'Khánh Hòa', 'Thành phố Nha Trang', '56'),
                ('Việt Nam', 'Ninh Thuận', 'Thành phố Phan Rang-Tháp Chàm', '58'),
                ('Việt Nam', 'Bình Thuận', 'Thành phố Phan Thiết', '60'),
                ('Việt Nam', 'Kon Tum', 'Thành phố Kon Tum', '62'),
                ('Việt Nam', 'Gia Lai', 'Thành phố Pleiku', '64'),
                ('Việt Nam', 'Đắk Lắk', 'Thành phố Buôn Ma Thuột', '66'),
                ('Việt Nam', 'Đắk Nông', 'Thành phố Gia Nghĩa', '67'),
                ('Việt Nam', 'Lâm Đồng', 'Thành phố Đà Lạt', '68'),
                
                -- Miền Nam
                ('Việt Nam', 'Bình Phước', 'Thành phố Đồng Xoài', '70'),
                ('Việt Nam', 'Tây Ninh', 'Thành phố Tây Ninh', '72'),
                ('Việt Nam', 'Bình Dương', 'Thành phố Thủ Dầu Một', '74'),
                ('Việt Nam', 'Đồng Nai', 'Thành phố Biên Hòa', '75'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Thành phố Vũng Tàu', '77'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thành phố', '79'),
                ('Việt Nam', 'Long An', 'Thành phố Tân An', '80'),
                ('Việt Nam', 'Tiền Giang', 'Thành phố Mỹ Tho', '82'),
                ('Việt Nam', 'Bến Tre', 'Thành phố Bến Tre', '83'),
                ('Việt Nam', 'Trà Vinh', 'Thành phố Trà Vinh', '84'),
                ('Việt Nam', 'Vĩnh Long', 'Thành phố Vĩnh Long', '86'),
                ('Việt Nam', 'Đồng Tháp', 'Thành phố Cao Lãnh', '87'),
                ('Việt Nam', 'An Giang', 'Thành phố Long Xuyên', '89'),
                ('Việt Nam', 'Kiên Giang', 'Thành phố Rạch Giá', '91'),
                ('Việt Nam', 'Cần Thơ', 'Thành phố', '92'),
                ('Việt Nam', 'Hậu Giang', 'Thành phố Vị Thanh', '93'),
                ('Việt Nam', 'Sóc Trăng', 'Thành phố Sóc Trăng', '94'),
                ('Việt Nam', 'Bạc Liêu', 'Thành phố Bạc Liêu', '95'),
                ('Việt Nam', 'Cà Mau', 'Thành phố Cà Mau', '96');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa data khi rollback
            migrationBuilder.Sql(@"DELETE FROM ""Locations"" WHERE ""Country"" = 'Việt Nam';");
        }
    }
}
