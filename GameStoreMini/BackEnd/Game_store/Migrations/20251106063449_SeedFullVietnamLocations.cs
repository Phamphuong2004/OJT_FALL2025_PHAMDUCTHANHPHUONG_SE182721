using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class SeedFullVietnamLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa data cũ trước
            migrationBuilder.Sql(@"DELETE FROM ""Locations"";");

            // Thêm đầy đủ 63 tỉnh/thành phố Việt Nam
            migrationBuilder.Sql(@"
                INSERT INTO ""Locations"" (""Country"", ""City"", ""District"", ""Code"") VALUES
                -- MIỀN BẮC
                -- Hà Nội (12 quận nội thành + 1 thị xã + 17 huyện)
                ('Việt Nam', 'Hà Nội', 'Ba Đình', 'HN-BD'),
                ('Việt Nam', 'Hà Nội', 'Hoàn Kiếm', 'HN-HK'),
                ('Việt Nam', 'Hà Nội', 'Đống Đa', 'HN-DD'),
                ('Việt Nam', 'Hà Nội', 'Hai Bà Trưng', 'HN-HBT'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'HN-HM'),
                ('Việt Nam', 'Hà Nội', 'Thanh Xuân', 'HN-TX'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'HN-LB'),
                ('Việt Nam', 'Hà Nội', 'Cầu Giấy', 'HN-CG'),
                ('Việt Nam', 'Hà Nội', 'Tây Hồ', 'HN-TH'),
                ('Việt Nam', 'Hà Nội', 'Nam Từ Liêm', 'HN-NTL'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'HN-BTL'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'HN-HDO'),
                
                -- Hải Phòng
                ('Việt Nam', 'Hải Phòng', 'Hồng Bàng', 'HP-HB'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'HP-LC'),
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'HP-NQ'),
                ('Việt Nam', 'Hải Phòng', 'Kiến An', 'HP-KA'),
                ('Việt Nam', 'Hải Phòng', 'Hải An', 'HP-HA'),
                ('Việt Nam', 'Hải Phòng', 'Đồ Sơn', 'HP-DS'),
                
                -- Quảng Ninh
                ('Việt Nam', 'Quảng Ninh', 'Hạ Long', 'QN-HL'),
                ('Việt Nam', 'Quảng Ninh', 'Móng Cái', 'QN-MC'),
                ('Việt Nam', 'Quảng Ninh', 'Cẩm Phả', 'QN-CP'),
                ('Việt Nam', 'Quảng Ninh', 'Uông Bí', 'QN-UB'),
                
                -- Các tỉnh Đồng bằng Bắc Bộ
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'BN-TP'),
                ('Việt Nam', 'Bắc Giang', 'Thành phố Bắc Giang', 'BG-TP'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'HD-TP'),
                ('Việt Nam', 'Hưng Yên', 'Thành phố Hưng Yên', 'HY-TP'),
                ('Việt Nam', 'Hà Nam', 'Phủ Lý', 'HNA-PL'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'ND-TP'),
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', 'TB-TP'),
                ('Việt Nam', 'Ninh Bình', 'Thành phố Ninh Bình', 'NB-TP'),
                ('Việt Nam', 'Vĩnh Phúc', 'Vĩnh Yên', 'VP-VY'),
                ('Việt Nam', 'Phú Thọ', 'Việt Trì', 'PT-VT'),
                
                -- Tây Bắc
                ('Việt Nam', 'Lào Cai', 'Thành phố Lào Cai', 'LC-TP'),
                ('Việt Nam', 'Lào Cai', 'Sa Pa', 'LC-SP'),
                ('Việt Nam', 'Điện Biên', 'Điện Biên Phủ', 'DB-DBP'),
                ('Việt Nam', 'Lai Châu', 'Thành phố Lai Châu', 'LCH-TP'),
                ('Việt Nam', 'Sơn La', 'Thành phố Sơn La', 'SL-TP'),
                ('Việt Nam', 'Yên Bái', 'Thành phố Yên Bái', 'YB-TP'),
                ('Việt Nam', 'Hòa Bình', 'Thành phố Hòa Bình', 'HB-TP'),
                
                -- Đông Bắc
                ('Việt Nam', 'Hà Giang', 'Thành phố Hà Giang', 'HG-TP'),
                ('Việt Nam', 'Cao Bằng', 'Thành phố Cao Bằng', 'CB-TP'),
                ('Việt Nam', 'Bắc Kạn', 'Thành phố Bắc Kạn', 'BK-TP'),
                ('Việt Nam', 'Tuyên Quang', 'Thành phố Tuyên Quang', 'TQ-TP'),
                ('Việt Nam', 'Lạng Sơn', 'Thành phố Lạng Sơn', 'LS-TP'),
                ('Việt Nam', 'Thái Nguyên', 'Thành phố Thái Nguyên', 'TN-TP'),
                
                -- MIỀN TRUNG
                -- Thanh Hóa
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'TH-TP'),
                ('Việt Nam', 'Thanh Hóa', 'Sầm Sơn', 'TH-SS'),
                
                -- Nghệ An
                ('Việt Nam', 'Nghệ An', 'Vinh', 'NA-V'),
                ('Việt Nam', 'Nghệ An', 'Cửa Lò', 'NA-CL'),
                
                -- Hà Tĩnh
                ('Việt Nam', 'Hà Tĩnh', 'Thành phố Hà Tĩnh', 'HT-TP'),
                
                -- Quảng Bình
                ('Việt Nam', 'Quảng Bình', 'Đồng Hới', 'QB-DH'),
                
                -- Quảng Trị
                ('Việt Nam', 'Quảng Trị', 'Đông Hà', 'QT-DH'),
                
                -- Thừa Thiên Huế
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'HUE-TP'),
                
                -- Đà Nẵng
                ('Việt Nam', 'Đà Nẵng', 'Hải Châu', 'DN-HC'),
                ('Việt Nam', 'Đà Nẵng', 'Thanh Khê', 'DN-TK'),
                ('Việt Nam', 'Đà Nẵng', 'Sơn Trà', 'DN-ST'),
                ('Việt Nam', 'Đà Nẵng', 'Ngũ Hành Sơn', 'DN-NHS'),
                ('Việt Nam', 'Đà Nẵng', 'Liên Chiểu', 'DN-LC'),
                ('Việt Nam', 'Đà Nẵng', 'Cẩm Lệ', 'DN-CL'),
                
                -- Quảng Nam
                ('Việt Nam', 'Quảng Nam', 'Tam Kỳ', 'QNM-TK'),
                ('Việt Nam', 'Quảng Nam', 'Hội An', 'QNM-HA'),
                
                -- Quảng Ngãi
                ('Việt Nam', 'Quảng Ngãi', 'Thành phố Quảng Ngãi', 'QNG-TP'),
                
                -- Bình Định
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'BD-QN'),
                
                -- Phú Yên
                ('Việt Nam', 'Phú Yên', 'Tuy Hòa', 'PY-TH'),
                
                -- Khánh Hòa
                ('Việt Nam', 'Khánh Hòa', 'Nha Trang', 'KH-NT'),
                ('Việt Nam', 'Khánh Hòa', 'Cam Ranh', 'KH-CR'),
                
                -- Ninh Thuận
                ('Việt Nam', 'Ninh Thuận', 'Phan Rang - Tháp Chàm', 'NT-PR'),
                
                -- Bình Thuận
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'BT-PT'),
                ('Việt Nam', 'Bình Thuận', 'La Gi', 'BT-LG'),
                
                -- Kon Tum
                ('Việt Nam', 'Kon Tum', 'Thành phố Kon Tum', 'KT-TP'),
                
                -- Gia Lai
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'GL-PL'),
                
                -- Đắk Lắk
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'DL-BMT'),
                
                -- Đắk Nông
                ('Việt Nam', 'Đắk Nông', 'Gia Nghĩa', 'DN-GN'),
                
                -- Lâm Đồng
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'LĐ-DL'),
                ('Việt Nam', 'Lâm Đồng', 'Bảo Lộc', 'LĐ-BL'),
                
                -- MIỀN NAM
                -- Hồ Chí Minh (16 quận + 5 huyện)
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 1', 'HCM-Q1'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 2', 'HCM-Q2'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 3', 'HCM-Q3'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'HCM-Q4'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 5', 'HCM-Q5'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'HCM-Q6'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 7', 'HCM-Q7'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'HCM-Q8'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'HCM-Q9'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'HCM-Q10'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'HCM-Q11'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 12', 'HCM-Q12'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'HCM-TD'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'HCM-GV'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'HCM-BT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'HCM-TB'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Phú', 'HCM-TP'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'HCM-PN'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Tân', 'HCM-BTN'),
                
                -- Đồng Nai
                ('Việt Nam', 'Đồng Nai', 'Biên Hòa', 'DNI-BH'),
                ('Việt Nam', 'Đồng Nai', 'Long Khánh', 'DNI-LK'),
                
                -- Bà Rịa - Vũng Tàu
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'BRVT-VT'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Bà Rịa', 'BRVT-BR'),
                
                -- Bình Dương
                ('Việt Nam', 'Bình Dương', 'Thủ Dầu Một', 'BD-TDM'),
                ('Việt Nam', 'Bình Dương', 'Dĩ An', 'BD-DA'),
                ('Việt Nam', 'Bình Dương', 'Thuận An', 'BD-TA'),
                
                -- Bình Phước
                ('Việt Nam', 'Bình Phước', 'Đồng Xoài', 'BP-DX'),
                
                -- Tây Ninh
                ('Việt Nam', 'Tây Ninh', 'Thành phố Tây Ninh', 'TN-TP'),
                
                -- Long An
                ('Việt Nam', 'Long An', 'Tân An', 'LA-TA'),
                
                -- Tiền Giang
                ('Việt Nam', 'Tiền Giang', 'Mỹ Tho', 'TG-MT'),
                
                -- Bến Tre
                ('Việt Nam', 'Bến Tre', 'Thành phố Bến Tre', 'BTE-TP'),
                
                -- Trà Vinh
                ('Việt Nam', 'Trà Vinh', 'Thành phố Trà Vinh', 'TV-TP'),
                
                -- Vĩnh Long
                ('Việt Nam', 'Vĩnh Long', 'Thành phố Vĩnh Long', 'VL-TP'),
                
                -- Đồng Tháp
                ('Việt Nam', 'Đồng Tháp', 'Cao Lãnh', 'DT-CL'),
                ('Việt Nam', 'Đồng Tháp', 'Sa Đéc', 'DT-SD'),
                
                -- An Giang
                ('Việt Nam', 'An Giang', 'Long Xuyên', 'AG-LX'),
                ('Việt Nam', 'An Giang', 'Châu Đốc', 'AG-CD'),
                
                -- Kiên Giang
                ('Việt Nam', 'Kiên Giang', 'Rạch Giá', 'KG-RG'),
                ('Việt Nam', 'Kiên Giang', 'Hà Tiên', 'KG-HT'),
                ('Việt Nam', 'Kiên Giang', 'Phú Quốc', 'KG-PQ'),
                
                -- Cần Thơ
                ('Việt Nam', 'Cần Thơ', 'Ninh Kiều', 'CT-NK'),
                ('Việt Nam', 'Cần Thơ', 'Cái Răng', 'CT-CR'),
                ('Việt Nam', 'Cần Thơ', 'Bình Thủy', 'CT-BT'),
                ('Việt Nam', 'Cần Thơ', 'Ô Môn', 'CT-OM'),
                
                -- Hậu Giang
                ('Việt Nam', 'Hậu Giang', 'Vị Thanh', 'HG-VT'),
                
                -- Sóc Trăng
                ('Việt Nam', 'Sóc Trăng', 'Thành phố Sóc Trăng', 'ST-TP'),
                
                -- Bạc Liêu
                ('Việt Nam', 'Bạc Liêu', 'Thành phố Bạc Liêu', 'BL-TP'),
                
                -- Cà Mau
                ('Việt Nam', 'Cà Mau', 'Thành phố Cà Mau', 'CM-TP');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""Locations"";");
        }
    }
}
