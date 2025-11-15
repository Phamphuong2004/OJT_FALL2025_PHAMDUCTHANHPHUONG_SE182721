using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class SeedRemainingProvincesWards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO ""Locations"" (""Country"", ""City"", ""District"", ""Ward"", ""Code"") VALUES
                
                -- ==================== GIA LAI - Pleiku ====================
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Yên Đỗ', 'GL-PL-YD'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Diên Hồng', 'GL-PL-DH'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Ia Kring', 'GL-PL-IK'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Hội Thương', 'GL-PL-HT'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Hội Phú', 'GL-PL-HP'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Phù Đổng', 'GL-PL-PD'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Hoa Lư', 'GL-PL-HL'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Tây Sơn', 'GL-PL-TS'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Thống Nhất', 'GL-PL-TN'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Đống Đa', 'GL-PL-DD'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Trà Bá', 'GL-PL-TB'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Thắng Lợi', 'GL-PL-TL'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Yên Thế', 'GL-PL-YT'),
                ('Việt Nam', 'Gia Lai', 'Pleiku', 'Phường Chi Lăng', 'GL-PL-CL'),

                -- ==================== ĐẮK LẮK - Buôn Ma Thuột ====================
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Tân Lập', 'DL-BMT-TL'),
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Tân Hòa', 'DL-BMT-TH'),
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Tân An', 'DL-BMT-TA'),
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Thống Nhất', 'DL-BMT-TN'),
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Thành Nhất', 'DL-BMT-TNh'),
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Thắng Lợi', 'DL-BMT-TLo'),
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Tân Lợi', 'DL-BMT-TLoi'),
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Thành Công', 'DL-BMT-TC'),
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Tân Thành', 'DL-BMT-TTh'),
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Tân Tiến', 'DL-BMT-TT'),
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Tự An', 'DL-BMT-TuA'),
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Ea Tam', 'DL-BMT-ET'),
                ('Việt Nam', 'Đắk Lắk', 'Buôn Ma Thuột', 'Phường Khánh Xuân', 'DL-BMT-KX'),

                -- ==================== KON TUM ====================
                ('Việt Nam', 'Kon Tum', 'Thành phố Kon Tum', 'Phường Quang Trung', 'KT-TP-QT'),
                ('Việt Nam', 'Kon Tum', 'Thành phố Kon Tum', 'Phường Duy Tân', 'KT-TP-DT'),
                ('Việt Nam', 'Kon Tum', 'Thành phố Kon Tum', 'Phường Quyết Thắng', 'KT-TP-QTh'),
                ('Việt Nam', 'Kon Tum', 'Thành phố Kon Tum', 'Phường Trường Chinh', 'KT-TP-TC'),
                ('Việt Nam', 'Kon Tum', 'Thành phố Kon Tum', 'Phường Thắng Lợi', 'KT-TP-TL'),
                ('Việt Nam', 'Kon Tum', 'Thành phố Kon Tum', 'Phường Ngô Mây', 'KT-TP-NM'),
                ('Việt Nam', 'Kon Tum', 'Thành phố Kon Tum', 'Phường Thống Nhất', 'KT-TP-TN'),
                ('Việt Nam', 'Kon Tum', 'Thành phố Kon Tum', 'Phường Lê Lợi', 'KT-TP-LL'),
                ('Việt Nam', 'Kon Tum', 'Thành phố Kon Tum', 'Phường Nguyễn Trãi', 'KT-TP-NT'),
                ('Việt Nam', 'Kon Tum', 'Thành phố Kon Tum', 'Phường Trần Hưng Đạo', 'KT-TP-THD'),

                -- ==================== ĐẮK NÔNG - Gia Nghĩa ====================
                ('Việt Nam', 'Đắk Nông', 'Gia Nghĩa', 'Phường Nghĩa Đức', 'DN-GN-ND'),
                ('Việt Nam', 'Đắk Nông', 'Gia Nghĩa', 'Phường Nghĩa Thành', 'DN-GN-NTh'),
                ('Việt Nam', 'Đắk Nông', 'Gia Nghĩa', 'Phường Nghĩa Phú', 'DN-GN-NP'),
                ('Việt Nam', 'Đắk Nông', 'Gia Nghĩa', 'Phường Nghĩa Tân', 'DN-GN-NTa'),
                ('Việt Nam', 'Đắk Nông', 'Gia Nghĩa', 'Phường Nghĩa Trung', 'DN-GN-NTr'),

                -- ==================== LÂM ĐỒNG - Bảo Lộc ====================
                ('Việt Nam', 'Lâm Đồng', 'Bảo Lộc', 'Phường 1', 'LĐ-BL-P1'),
                ('Việt Nam', 'Lâm Đồng', 'Bảo Lộc', 'Phường 2', 'LĐ-BL-P2'),
                ('Việt Nam', 'Lâm Đồng', 'Bảo Lộc', 'Phường B''Lao', 'LĐ-BL-BL'),
                ('Việt Nam', 'Lâm Đồng', 'Bảo Lộc', 'Phường Lộc Phát', 'LĐ-BL-LP'),
                ('Việt Nam', 'Lâm Đồng', 'Bảo Lộc', 'Phường Lộc Tiến', 'LĐ-BL-LT'),
                ('Việt Nam', 'Lâm Đồng', 'Bảo Lộc', 'Phường Lộc Sơn', 'LĐ-BL-LS'),

                -- ==================== QUẢNG NGÃI ====================
                ('Việt Nam', 'Quảng Ngãi', 'Thành phố Quảng Ngãi', 'Phường Lê Hồng Phong', 'QNG-TP-LHP'),
                ('Việt Nam', 'Quảng Ngãi', 'Thành phố Quảng Ngãi', 'Phường Trần Phú', 'QNG-TP-TP'),
                ('Việt Nam', 'Quảng Ngãi', 'Thành phố Quảng Ngãi', 'Phường Quảng Phú', 'QNG-TP-QP'),
                ('Việt Nam', 'Quảng Ngãi', 'Thành phố Quảng Ngãi', 'Phường Nghĩa Chánh', 'QNG-TP-NC'),
                ('Việt Nam', 'Quảng Ngãi', 'Thành phố Quảng Ngãi', 'Phường Trần Hưng Đạo', 'QNG-TP-THD'),
                ('Việt Nam', 'Quảng Ngãi', 'Thành phố Quảng Ngãi', 'Phường Chánh Lộ', 'QNG-TP-CL'),
                ('Việt Nam', 'Quảng Ngãi', 'Thành phố Quảng Ngãi', 'Phường Nghĩa Lộ', 'QNG-TP-NL'),
                ('Việt Nam', 'Quảng Ngãi', 'Thành phố Quảng Ngãi', 'Phường Trương Quang Trọng', 'QNG-TP-TQT'),

                -- ==================== HÀ TĨNH ====================
                ('Việt Nam', 'Hà Tĩnh', 'Thành phố Hà Tĩnh', 'Phường Trần Phú', 'HT-TP-TP'),
                ('Việt Nam', 'Hà Tĩnh', 'Thành phố Hà Tĩnh', 'Phường Nam Hà', 'HT-TP-NH'),
                ('Việt Nam', 'Hà Tĩnh', 'Thành phố Hà Tĩnh', 'Phường Bắc Hà', 'HT-TP-BH'),
                ('Việt Nam', 'Hà Tĩnh', 'Thành phố Hà Tĩnh', 'Phường Nguyễn Du', 'HT-TP-ND'),
                ('Việt Nam', 'Hà Tĩnh', 'Thành phố Hà Tĩnh', 'Phường Tân Giang', 'HT-TP-TG'),
                ('Việt Nam', 'Hà Tĩnh', 'Thành phố Hà Tĩnh', 'Phường Đại Nài', 'HT-TP-DN'),
                ('Việt Nam', 'Hà Tĩnh', 'Thành phố Hà Tĩnh', 'Phường Hà Huy Tập', 'HT-TP-HHT'),
                ('Việt Nam', 'Hà Tĩnh', 'Thành phố Hà Tĩnh', 'Phường Thạch Quý', 'HT-TP-TQ'),
                ('Việt Nam', 'Hà Tĩnh', 'Thành phố Hà Tĩnh', 'Phường Thạch Linh', 'HT-TP-TL'),

                -- ==================== QUẢNG BÌNH - Đồng Hới ====================
                ('Việt Nam', 'Quảng Bình', 'Đồng Hới', 'Phường Hải Thành', 'QB-DH-HT'),
                ('Việt Nam', 'Quảng Bình', 'Đồng Hới', 'Phường Đồng Phú', 'QB-DH-DP'),
                ('Việt Nam', 'Quảng Bình', 'Đồng Hới', 'Phường Bắc Lý', 'QB-DH-BL'),
                ('Việt Nam', 'Quảng Bình', 'Đồng Hới', 'Phường Nam Lý', 'QB-DH-NL'),
                ('Việt Nam', 'Quảng Bình', 'Đồng Hới', 'Phường Đồng Sơn', 'QB-DH-DS'),
                ('Việt Nam', 'Quảng Bình', 'Đồng Hới', 'Phường Phú Hải', 'QB-DH-PH'),
                ('Việt Nam', 'Quảng Bình', 'Đồng Hới', 'Phường Bắc Nghĩa', 'QB-DH-BN'),
                ('Việt Nam', 'Quảng Bình', 'Đồng Hới', 'Phường Đức Ninh Đông', 'QB-DH-DND'),

                -- ==================== QUẢNG TRỊ - Đông Hà ====================
                ('Việt Nam', 'Quảng Trị', 'Đông Hà', 'Phường Đông Giang', 'QT-DH-DG'),
                ('Việt Nam', 'Quảng Trị', 'Đông Hà', 'Phường 1', 'QT-DH-P1'),
                ('Việt Nam', 'Quảng Trị', 'Đông Hà', 'Phường 2', 'QT-DH-P2'),
                ('Việt Nam', 'Quảng Trị', 'Đông Hà', 'Phường 3', 'QT-DH-P3'),
                ('Việt Nam', 'Quảng Trị', 'Đông Hà', 'Phường 4', 'QT-DH-P4'),
                ('Việt Nam', 'Quảng Trị', 'Đông Hà', 'Phường 5', 'QT-DH-P5'),
                ('Việt Nam', 'Quảng Trị', 'Đông Hà', 'Phường Đông Lễ', 'QT-DH-DL'),
                ('Việt Nam', 'Quảng Trị', 'Đông Hà', 'Phường Đông Lương', 'QT-DH-DLu'),

                -- ==================== LÀO CAI ====================
                ('Việt Nam', 'Lào Cai', 'Thành phố Lào Cai', 'Phường Duyên Hải', 'LC-TP-DH'),
                ('Việt Nam', 'Lào Cai', 'Thành phố Lào Cai', 'Phường Lào Cai', 'LC-TP-LC'),
                ('Việt Nam', 'Lào Cai', 'Thành phố Lào Cai', 'Phường Cốc Lếu', 'LC-TP-CL'),
                ('Việt Nam', 'Lào Cai', 'Thành phố Lào Cai', 'Phường Kim Tân', 'LC-TP-KT'),
                ('Việt Nam', 'Lào Cai', 'Thành phố Lào Cai', 'Phường Bắc Lệnh', 'LC-TP-BL'),
                ('Việt Nam', 'Lào Cai', 'Thành phố Lào Cai', 'Phường Pom Hán', 'LC-TP-PH'),
                ('Việt Nam', 'Lào Cai', 'Thành phố Lào Cai', 'Phường Xuân Tăng', 'LC-TP-XT'),
                ('Việt Nam', 'Lào Cai', 'Thành phố Lào Cai', 'Phường Bình Minh', 'LC-TP-BM'),

                -- ==================== LÀO CAI - Sa Pa ====================
                ('Việt Nam', 'Lào Cai', 'Sa Pa', 'Phường Sa Pa', 'LC-SP-SP'),
                ('Việt Nam', 'Lào Cai', 'Sa Pa', 'Phường Sa Pả', 'LC-SP-SPa'),
                ('Việt Nam', 'Lào Cai', 'Sa Pa', 'Phường Ô Quý Hồ', 'LC-SP-OQH'),
                ('Việt Nam', 'Lào Cai', 'Sa Pa', 'Phường Phan Si Păng', 'LC-SP-PSP'),
                ('Việt Nam', 'Lào Cai', 'Sa Pa', 'Phường Hàm Rồng', 'LC-SP-HR'),

                -- ==================== YÊN BÁI ====================
                ('Việt Nam', 'Yên Bái', 'Thành phố Yên Bái', 'Phường Yên Thịnh', 'YB-TP-YT'),
                ('Việt Nam', 'Yên Bái', 'Thành phố Yên Bái', 'Phường Yên Ninh', 'YB-TP-YN'),
                ('Việt Nam', 'Yên Bái', 'Thành phố Yên Bái', 'Phường Minh Tân', 'YB-TP-MT'),
                ('Việt Nam', 'Yên Bái', 'Thành phố Yên Bái', 'Phường Nguyễn Thái Học', 'YB-TP-NTH'),
                ('Việt Nam', 'Yên Bái', 'Thành phố Yên Bái', 'Phường Đồng Tâm', 'YB-TP-DT'),
                ('Việt Nam', 'Yên Bái', 'Thành phố Yên Bái', 'Phường Nguyễn Phúc', 'YB-TP-NP'),
                ('Việt Nam', 'Yên Bái', 'Thành phố Yên Bái', 'Phường Hồng Hà', 'YB-TP-HH'),

                -- ==================== ĐIỆN BIÊN - Điện Biên Phủ ====================
                ('Việt Nam', 'Điện Biên', 'Điện Biên Phủ', 'Phường Noong Bua', 'DB-DBP-NB'),
                ('Việt Nam', 'Điện Biên', 'Điện Biên Phủ', 'Phường Him Lam', 'DB-DBP-HL'),
                ('Việt Nam', 'Điện Biên', 'Điện Biên Phủ', 'Phường Thanh Bình', 'DB-DBP-TB'),
                ('Việt Nam', 'Điện Biên', 'Điện Biên Phủ', 'Phường Tân Thanh', 'DB-DBP-TT'),
                ('Việt Nam', 'Điện Biên', 'Điện Biên Phủ', 'Phường Mường Thanh', 'DB-DBP-MT'),
                ('Việt Nam', 'Điện Biên', 'Điện Biên Phủ', 'Phường Nam Thanh', 'DB-DBP-NT'),
                ('Việt Nam', 'Điện Biên', 'Điện Biên Phủ', 'Phường Thanh Trường', 'DB-DBP-TTr'),

                -- ==================== HÒA BÌNH ====================
                ('Việt Nam', 'Hòa Bình', 'Thành phố Hòa Bình', 'Phường Thái Bình', 'HB-TP-TB'),
                ('Việt Nam', 'Hòa Bình', 'Thành phố Hòa Bình', 'Phường Tân Hòa', 'HB-TP-TH'),
                ('Việt Nam', 'Hòa Bình', 'Thành phố Hòa Bình', 'Phường Đồng Tiến', 'HB-TP-DT'),
                ('Việt Nam', 'Hòa Bình', 'Thành phố Hòa Bình', 'Phường Phương Lâm', 'HB-TP-PL'),
                ('Việt Nam', 'Hòa Bình', 'Thành phố Hòa Bình', 'Phường Tân Thịnh', 'HB-TP-TT'),
                ('Việt Nam', 'Hòa Bình', 'Thành phố Hòa Bình', 'Phường Thịnh Lang', 'HB-TP-TL'),
                ('Việt Nam', 'Hòa Bình', 'Thành phố Hòa Bình', 'Phường Hữu Nghị', 'HB-TP-HN'),

                -- ==================== SƠN LA ====================
                ('Việt Nam', 'Sơn La', 'Thành phố Sơn La', 'Phường Chiềng Lề', 'SL-TP-CL'),
                ('Việt Nam', 'Sơn La', 'Thành phố Sơn La', 'Phường Tô Hiệu', 'SL-TP-TH'),
                ('Việt Nam', 'Sơn La', 'Thành phố Sơn La', 'Phường Quyết Thắng', 'SL-TP-QT'),
                ('Việt Nam', 'Sơn La', 'Thành phố Sơn La', 'Phường Quyết Tâm', 'SL-TP-QTa'),
                ('Việt Nam', 'Sơn La', 'Thành phố Sơn La', 'Phường Chiềng Cơi', 'SL-TP-CC'),
                ('Việt Nam', 'Sơn La', 'Thành phố Sơn La', 'Phường Chiềng Đen', 'SL-TP-CD'),
                ('Việt Nam', 'Sơn La', 'Thành phố Sơn La', 'Phường Chiềng An', 'SL-TP-CA'),

                -- ==================== THÁI NGUYÊN ====================
                ('Việt Nam', 'Thái Nguyên', 'Thành phố Thái Nguyên', 'Phường Quán Triều', 'TN-TP-QT'),
                ('Việt Nam', 'Thái Nguyên', 'Thành phố Thái Nguyên', 'Phường Quang Vinh', 'TN-TP-QV'),
                ('Việt Nam', 'Thái Nguyên', 'Thành phố Thái Nguyên', 'Phường Túc Duyên', 'TN-TP-TD'),
                ('Việt Nam', 'Thái Nguyên', 'Thành phố Thái Nguyên', 'Phường Hoàng Văn Thụ', 'TN-TP-HVT'),
                ('Việt Nam', 'Thái Nguyên', 'Thành phố Thái Nguyên', 'Phường Trưng Vương', 'TN-TP-TV'),
                ('Việt Nam', 'Thái Nguyên', 'Thành phố Thái Nguyên', 'Phường Quang Trung', 'TN-TP-QTr'),
                ('Việt Nam', 'Thái Nguyên', 'Thành phố Thái Nguyên', 'Phường Phan Đình Phùng', 'TN-TP-PDP'),
                ('Việt Nam', 'Thái Nguyên', 'Thành phố Thái Nguyên', 'Phường Tân Thịnh', 'TN-TP-TT'),
                ('Việt Nam', 'Thái Nguyên', 'Thành phố Thái Nguyên', 'Phường Thịnh Đán', 'TN-TP-TĐ'),
                ('Việt Nam', 'Thái Nguyên', 'Thành phố Thái Nguyên', 'Phường Đồng Quang', 'TN-TP-DQ'),

                -- ==================== BÌNH PHƯỚC - Đồng Xoài ====================
                ('Việt Nam', 'Bình Phước', 'Đồng Xoài', 'Phường Tân Bình', 'BP-DX-TB'),
                ('Việt Nam', 'Bình Phước', 'Đồng Xoài', 'Phường Tân Đồng', 'BP-DX-TD'),
                ('Việt Nam', 'Bình Phước', 'Đồng Xoài', 'Phường Tân Xuân', 'BP-DX-TX'),
                ('Việt Nam', 'Bình Phước', 'Đồng Xoài', 'Phường Tân Phú', 'BP-DX-TP'),
                ('Việt Nam', 'Bình Phước', 'Đồng Xoài', 'Phường Tân Thiện', 'BP-DX-TT'),
                ('Việt Nam', 'Bình Phước', 'Đồng Xoài', 'Phường Tân Thành', 'BP-DX-TTh'),
                ('Việt Nam', 'Bình Phước', 'Đồng Xoài', 'Phường Tiến Thành', 'BP-DX-TiT'),

                -- ==================== TÂY NINH ====================
                ('Việt Nam', 'Tây Ninh', 'Thành phố Tây Ninh', 'Phường 1', 'TN-TP-P1'),
                ('Việt Nam', 'Tây Ninh', 'Thành phố Tây Ninh', 'Phường 2', 'TN-TP-P2'),
                ('Việt Nam', 'Tây Ninh', 'Thành phố Tây Ninh', 'Phường 3', 'TN-TP-P3'),
                ('Việt Nam', 'Tây Ninh', 'Thành phố Tây Ninh', 'Phường 4', 'TN-TP-P4'),
                ('Việt Nam', 'Tây Ninh', 'Thành phố Tây Ninh', 'Phường Hiệp Ninh', 'TN-TP-HN'),
                ('Việt Nam', 'Tây Ninh', 'Thành phố Tây Ninh', 'Phường Ninh Sơn', 'TN-TP-NS'),
                ('Việt Nam', 'Tây Ninh', 'Thành phố Tây Ninh', 'Phường Ninh Thạnh', 'TN-TP-NTh'),

                -- ==================== LONG AN - Tân An ====================
                ('Việt Nam', 'Long An', 'Tân An', 'Phường 1', 'LA-TA-P1'),
                ('Việt Nam', 'Long An', 'Tân An', 'Phường 2', 'LA-TA-P2'),
                ('Việt Nam', 'Long An', 'Tân An', 'Phường 3', 'LA-TA-P3'),
                ('Việt Nam', 'Long An', 'Tân An', 'Phường 4', 'LA-TA-P4'),
                ('Việt Nam', 'Long An', 'Tân An', 'Phường 5', 'LA-TA-P5'),
                ('Việt Nam', 'Long An', 'Tân An', 'Phường 6', 'LA-TA-P6'),
                ('Việt Nam', 'Long An', 'Tân An', 'Phường 7', 'LA-TA-P7'),
                ('Việt Nam', 'Long An', 'Tân An', 'Phường Khánh Hậu', 'LA-TA-KH'),
                ('Việt Nam', 'Long An', 'Tân An', 'Phường Tân Khánh', 'LA-TA-TK'),

                -- ==================== BẾN TRE ====================
                ('Việt Nam', 'Bến Tre', 'Thành phố Bến Tre', 'Phường 1', 'BTE-TP-P1'),
                ('Việt Nam', 'Bến Tre', 'Thành phố Bến Tre', 'Phường 2', 'BTE-TP-P2'),
                ('Việt Nam', 'Bến Tre', 'Thành phố Bến Tre', 'Phường 3', 'BTE-TP-P3'),
                ('Việt Nam', 'Bến Tre', 'Thành phố Bến Tre', 'Phường 4', 'BTE-TP-P4'),
                ('Việt Nam', 'Bến Tre', 'Thành phố Bến Tre', 'Phường 5', 'BTE-TP-P5'),
                ('Việt Nam', 'Bến Tre', 'Thành phố Bến Tre', 'Phường 6', 'BTE-TP-P6'),
                ('Việt Nam', 'Bến Tre', 'Thành phố Bến Tre', 'Phường 7', 'BTE-TP-P7'),
                ('Việt Nam', 'Bến Tre', 'Thành phố Bến Tre', 'Phường 8', 'BTE-TP-P8'),
                ('Việt Nam', 'Bến Tre', 'Thành phố Bến Tre', 'Phường Phú Khương', 'BTE-TP-PK'),
                ('Việt Nam', 'Bến Tre', 'Thành phố Bến Tre', 'Phường Phú Tân', 'BTE-TP-PT'),

                -- ==================== TRÀ VINH ====================
                ('Việt Nam', 'Trà Vinh', 'Thành phố Trà Vinh', 'Phường 1', 'TV-TP-P1'),
                ('Việt Nam', 'Trà Vinh', 'Thành phố Trà Vinh', 'Phường 2', 'TV-TP-P2'),
                ('Việt Nam', 'Trà Vinh', 'Thành phố Trà Vinh', 'Phường 3', 'TV-TP-P3'),
                ('Việt Nam', 'Trà Vinh', 'Thành phố Trà Vinh', 'Phường 4', 'TV-TP-P4'),
                ('Việt Nam', 'Trà Vinh', 'Thành phố Trà Vinh', 'Phường 5', 'TV-TP-P5'),
                ('Việt Nam', 'Trà Vinh', 'Thành phố Trà Vinh', 'Phường 6', 'TV-TP-P6'),
                ('Việt Nam', 'Trà Vinh', 'Thành phố Trà Vinh', 'Phường 7', 'TV-TP-P7'),
                ('Việt Nam', 'Trà Vinh', 'Thành phố Trà Vinh', 'Phường 8', 'TV-TP-P8'),
                ('Việt Nam', 'Trà Vinh', 'Thành phố Trà Vinh', 'Phường 9', 'TV-TP-P9'),

                -- ==================== VĨNH LONG ====================
                ('Việt Nam', 'Vĩnh Long', 'Thành phố Vĩnh Long', 'Phường 1', 'VL-TP-P1'),
                ('Việt Nam', 'Vĩnh Long', 'Thành phố Vĩnh Long', 'Phường 2', 'VL-TP-P2'),
                ('Việt Nam', 'Vĩnh Long', 'Thành phố Vĩnh Long', 'Phường 3', 'VL-TP-P3'),
                ('Việt Nam', 'Vĩnh Long', 'Thành phố Vĩnh Long', 'Phường 4', 'VL-TP-P4'),
                ('Việt Nam', 'Vĩnh Long', 'Thành phố Vĩnh Long', 'Phường 5', 'VL-TP-P5'),
                ('Việt Nam', 'Vĩnh Long', 'Thành phố Vĩnh Long', 'Phường 8', 'VL-TP-P8'),
                ('Việt Nam', 'Vĩnh Long', 'Thành phố Vĩnh Long', 'Phường 9', 'VL-TP-P9'),
                ('Việt Nam', 'Vĩnh Long', 'Thành phố Vĩnh Long', 'Phường Tân Ngãi', 'VL-TP-TN'),
                ('Việt Nam', 'Vĩnh Long', 'Thành phố Vĩnh Long', 'Phường Tân Hòa', 'VL-TP-TH'),
                ('Việt Nam', 'Vĩnh Long', 'Thành phố Vĩnh Long', 'Phường Tân Hội', 'VL-TP-THo'),

                -- ==================== ĐỒNG THÁP - Cao Lãnh ====================
                ('Việt Nam', 'Đồng Tháp', 'Cao Lãnh', 'Phường 1', 'DT-CL-P1'),
                ('Việt Nam', 'Đồng Tháp', 'Cao Lãnh', 'Phường 2', 'DT-CL-P2'),
                ('Việt Nam', 'Đồng Tháp', 'Cao Lãnh', 'Phường 3', 'DT-CL-P3'),
                ('Việt Nam', 'Đồng Tháp', 'Cao Lãnh', 'Phường 4', 'DT-CL-P4'),
                ('Việt Nam', 'Đồng Tháp', 'Cao Lãnh', 'Phường 6', 'DT-CL-P6'),
                ('Việt Nam', 'Đồng Tháp', 'Cao Lãnh', 'Phường 10', 'DT-CL-P10'),
                ('Việt Nam', 'Đồng Tháp', 'Cao Lãnh', 'Phường 11', 'DT-CL-P11'),
                ('Việt Nam', 'Đồng Tháp', 'Cao Lãnh', 'Phường Mỹ Phú', 'DT-CL-MP'),

                -- ==================== AN GIANG - Châu Đốc ====================
                ('Việt Nam', 'An Giang', 'Châu Đốc', 'Phường Châu Phú B', 'AG-CD-CPB'),
                ('Việt Nam', 'An Giang', 'Châu Đốc', 'Phường Châu Phú A', 'AG-CD-CPA'),
                ('Việt Nam', 'An Giang', 'Châu Đốc', 'Phường Vĩnh Mỹ', 'AG-CD-VM'),
                ('Việt Nam', 'An Giang', 'Châu Đốc', 'Phường Núi Sam', 'AG-CD-NS'),
                ('Việt Nam', 'An Giang', 'Châu Đốc', 'Phường Vĩnh Ngươn', 'AG-CD-VN'),

                -- ==================== KIÊN GIANG - Hà Tiên ====================
                ('Việt Nam', 'Kiên Giang', 'Hà Tiên', 'Phường Tô Châu', 'KG-HT-TC'),
                ('Việt Nam', 'Kiên Giang', 'Hà Tiên', 'Phường Đông Hồ', 'KG-HT-DH'),
                ('Việt Nam', 'Kiên Giang', 'Hà Tiên', 'Phường Bình San', 'KG-HT-BS'),
                ('Việt Nam', 'Kiên Giang', 'Hà Tiên', 'Phường Mỹ Đức', 'KG-HT-MD'),

                -- ==================== KIÊN GIANG - Phú Quốc ====================
                ('Việt Nam', 'Kiên Giang', 'Phú Quốc', 'Phường Dương Đông', 'KG-PQ-DD'),
                ('Việt Nam', 'Kiên Giang', 'Phú Quốc', 'Phường An Thới', 'KG-PQ-AT'),
                ('Việt Nam', 'Kiên Giang', 'Phú Quốc', 'Xã Cửa Cạn', 'KG-PQ-CC'),
                ('Việt Nam', 'Kiên Giang', 'Phú Quốc', 'Xã Gành Dầu', 'KG-PQ-GD'),
                ('Việt Nam', 'Kiên Giang', 'Phú Quốc', 'Xã Cửa Dương', 'KG-PQ-CDu'),
                ('Việt Nam', 'Kiên Giang', 'Phú Quốc', 'Xã Hàm Ninh', 'KG-PQ-HN'),

                -- ==================== HẬU GIANG - Vị Thanh ====================
                ('Việt Nam', 'Hậu Giang', 'Vị Thanh', 'Phường I', 'HG-VT-P1'),
                ('Việt Nam', 'Hậu Giang', 'Vị Thanh', 'Phường III', 'HG-VT-P3'),
                ('Việt Nam', 'Hậu Giang', 'Vị Thanh', 'Phường IV', 'HG-VT-P4'),
                ('Việt Nam', 'Hậu Giang', 'Vị Thanh', 'Phường V', 'HG-VT-P5'),
                ('Việt Nam', 'Hậu Giang', 'Vị Thanh', 'Phường VII', 'HG-VT-P7'),

                -- ==================== SÓC TRĂNG ====================
                ('Việt Nam', 'Sóc Trăng', 'Thành phố Sóc Trăng', 'Phường 1', 'ST-TP-P1'),
                ('Việt Nam', 'Sóc Trăng', 'Thành phố Sóc Trăng', 'Phường 2', 'ST-TP-P2'),
                ('Việt Nam', 'Sóc Trăng', 'Thành phố Sóc Trăng', 'Phường 3', 'ST-TP-P3'),
                ('Việt Nam', 'Sóc Trăng', 'Thành phố Sóc Trăng', 'Phường 5', 'ST-TP-P5'),
                ('Việt Nam', 'Sóc Trăng', 'Thành phố Sóc Trăng', 'Phường 6', 'ST-TP-P6'),
                ('Việt Nam', 'Sóc Trăng', 'Thành phố Sóc Trăng', 'Phường 7', 'ST-TP-P7'),
                ('Việt Nam', 'Sóc Trăng', 'Thành phố Sóc Trăng', 'Phường 8', 'ST-TP-P8'),
                ('Việt Nam', 'Sóc Trăng', 'Thành phố Sóc Trăng', 'Phường 9', 'ST-TP-P9'),
                ('Việt Nam', 'Sóc Trăng', 'Thành phố Sóc Trăng', 'Phường 10', 'ST-TP-P10'),

                -- ==================== BẠC LIÊU ====================
                ('Việt Nam', 'Bạc Liêu', 'Thành phố Bạc Liêu', 'Phường 1', 'BL-TP-P1'),
                ('Việt Nam', 'Bạc Liêu', 'Thành phố Bạc Liêu', 'Phường 2', 'BL-TP-P2'),
                ('Việt Nam', 'Bạc Liêu', 'Thành phố Bạc Liêu', 'Phường 3', 'BL-TP-P3'),
                ('Việt Nam', 'Bạc Liêu', 'Thành phố Bạc Liêu', 'Phường 5', 'BL-TP-P5'),
                ('Việt Nam', 'Bạc Liêu', 'Thành phố Bạc Liêu', 'Phường 7', 'BL-TP-P7'),
                ('Việt Nam', 'Bạc Liêu', 'Thành phố Bạc Liêu', 'Phường 8', 'BL-TP-P8'),
                ('Việt Nam', 'Bạc Liêu', 'Thành phố Bạc Liêu', 'Phường Nhà Mát', 'BL-TP-NM'),

                -- ==================== CÀ MAU ====================
                ('Việt Nam', 'Cà Mau', 'Thành phố Cà Mau', 'Phường 1', 'CM-TP-P1'),
                ('Việt Nam', 'Cà Mau', 'Thành phố Cà Mau', 'Phường 2', 'CM-TP-P2'),
                ('Việt Nam', 'Cà Mau', 'Thành phố Cà Mau', 'Phường 4', 'CM-TP-P4'),
                ('Việt Nam', 'Cà Mau', 'Thành phố Cà Mau', 'Phường 5', 'CM-TP-P5'),
                ('Việt Nam', 'Cà Mau', 'Thành phố Cà Mau', 'Phường 6', 'CM-TP-P6'),
                ('Việt Nam', 'Cà Mau', 'Thành phố Cà Mau', 'Phường 7', 'CM-TP-P7'),
                ('Việt Nam', 'Cà Mau', 'Thành phố Cà Mau', 'Phường 8', 'CM-TP-P8'),
                ('Việt Nam', 'Cà Mau', 'Thành phố Cà Mau', 'Phường 9', 'CM-TP-P9'),
                ('Việt Nam', 'Cà Mau', 'Thành phố Cà Mau', 'Phường Tân Thành', 'CM-TP-TT'),
                ('Việt Nam', 'Cà Mau', 'Thành phố Cà Mau', 'Phường Tân Xuyên', 'CM-TP-TX');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""Locations"" WHERE ""Ward"" IS NOT NULL 
                AND (""Code"" LIKE '%GL-PL-%' OR ""Code"" LIKE '%DL-BMT-%' OR ""Code"" LIKE '%KT-TP-%'
                OR ""Code"" LIKE '%DN-GN-%' OR ""Code"" LIKE '%LĐ-BL-%' OR ""Code"" LIKE '%QNG-TP-%'
                OR ""Code"" LIKE '%HT-TP-%' OR ""Code"" LIKE '%QB-DH-%' OR ""Code"" LIKE '%QT-DH-%'
                OR ""Code"" LIKE '%LC-TP-%' OR ""Code"" LIKE '%LC-SP-%' OR ""Code"" LIKE '%YB-TP-%'
                OR ""Code"" LIKE '%DB-DBP-%' OR ""Code"" LIKE '%HB-TP-%' OR ""Code"" LIKE '%SL-TP-%'
                OR ""Code"" LIKE '%TN-TP-%' OR ""Code"" LIKE '%BP-DX-%' OR ""Code"" LIKE '%LA-TA-%'
                OR ""Code"" LIKE '%BTE-TP-%' OR ""Code"" LIKE '%TV-TP-%' OR ""Code"" LIKE '%VL-TP-%'
                OR ""Code"" LIKE '%DT-CL-%' OR ""Code"" LIKE '%AG-CD-%' OR ""Code"" LIKE '%KG-HT-%'
                OR ""Code"" LIKE '%KG-PQ-%' OR ""Code"" LIKE '%HG-VT-%' OR ""Code"" LIKE '%ST-TP-%'
                OR ""Code"" LIKE '%BL-TP-%' OR ""Code"" LIKE '%CM-TP-%');");
        }
    }
}
