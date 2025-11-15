using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class SeedExtendedVietnamWardsAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO ""Locations"" (""Country"", ""City"", ""District"", ""Ward"", ""Code"") VALUES
                
                -- ==================== QUẢNG NINH - Móng Cái ====================
                ('Việt Nam', 'Quảng Ninh', 'Móng Cái', 'Phường Ka Long', 'QN-MC-KL'),
                ('Việt Nam', 'Quảng Ninh', 'Móng Cái', 'Phường Trần Phú', 'QN-MC-TP'),
                ('Việt Nam', 'Quảng Ninh', 'Móng Cái', 'Phường Ninh Dương', 'QN-MC-ND'),
                ('Việt Nam', 'Quảng Ninh', 'Móng Cái', 'Phường Hoà Lạc', 'QN-MC-HL'),
                ('Việt Nam', 'Quảng Ninh', 'Móng Cái', 'Phường Trà Cổ', 'QN-MC-TC'),
                ('Việt Nam', 'Quảng Ninh', 'Móng Cái', 'Phường Hải Sơn', 'QN-MC-HS'),
                ('Việt Nam', 'Quảng Ninh', 'Móng Cái', 'Phường Bình Ngọc', 'QN-MC-BN'),
                ('Việt Nam', 'Quảng Ninh', 'Móng Cái', 'Phường Hải Đông', 'QN-MC-HD'),
                ('Việt Nam', 'Quảng Ninh', 'Móng Cái', 'Phường Hải Tiến', 'QN-MC-HT'),
                ('Việt Nam', 'Quảng Ninh', 'Móng Cái', 'Phường Hải Yên', 'QN-MC-HY'),

                -- ==================== QUẢNG NINH - Cẩm Phả ====================
                ('Việt Nam', 'Quảng Ninh', 'Cẩm Phả', 'Phường Mông Dương', 'QN-CP-MD'),
                ('Việt Nam', 'Quảng Ninh', 'Cẩm Phả', 'Phường Cửa Ông', 'QN-CP-CO'),
                ('Việt Nam', 'Quảng Ninh', 'Cẩm Phả', 'Phường Cẩm Sơn', 'QN-CP-CS'),
                ('Việt Nam', 'Quảng Ninh', 'Cẩm Phả', 'Phường Cẩm Đông', 'QN-CP-CD'),
                ('Việt Nam', 'Quảng Ninh', 'Cẩm Phả', 'Phường Cẩm Phú', 'QN-CP-CP'),
                ('Việt Nam', 'Quảng Ninh', 'Cẩm Phả', 'Phường Cẩm Tây', 'QN-CP-CT'),
                ('Việt Nam', 'Quảng Ninh', 'Cẩm Phả', 'Phường Quang Hanh', 'QN-CP-QH'),
                ('Việt Nam', 'Quảng Ninh', 'Cẩm Phả', 'Phường Cẩm Thịnh', 'QN-CP-CTH'),
                ('Việt Nam', 'Quảng Ninh', 'Cẩm Phả', 'Phường Cẩm Thủy', 'QN-CP-CThu'),
                ('Việt Nam', 'Quảng Ninh', 'Cẩm Phả', 'Phường Cẩm Thạch', 'QN-CP-CTha'),

                -- ==================== QUẢNG NINH - Uông Bí ====================
                ('Việt Nam', 'Quảng Ninh', 'Uông Bí', 'Phường Vàng Danh', 'QN-UB-VD'),
                ('Việt Nam', 'Quảng Ninh', 'Uông Bí', 'Phường Thanh Sơn', 'QN-UB-TS'),
                ('Việt Nam', 'Quảng Ninh', 'Uông Bí', 'Phường Bắc Sơn', 'QN-UB-BS'),
                ('Việt Nam', 'Quảng Ninh', 'Uông Bí', 'Phường Quang Trung', 'QN-UB-QT'),
                ('Việt Nam', 'Quảng Ninh', 'Uông Bí', 'Phường Trưng Vương', 'QN-UB-TV'),
                ('Việt Nam', 'Quảng Ninh', 'Uông Bí', 'Phường Nam Khê', 'QN-UB-NK'),
                ('Việt Nam', 'Quảng Ninh', 'Uông Bí', 'Phường Yên Thanh', 'QN-UB-YT'),

                -- ==================== BẮC GIANG ====================
                ('Việt Nam', 'Bắc Giang', 'Thành phố Bắc Giang', 'Phường Thọ Xương', 'BG-TP-TX'),
                ('Việt Nam', 'Bắc Giang', 'Thành phố Bắc Giang', 'Phường Trần Nguyên Hãn', 'BG-TP-TNH'),
                ('Việt Nam', 'Bắc Giang', 'Thành phố Bắc Giang', 'Phường Ngô Quyền', 'BG-TP-NQ'),
                ('Việt Nam', 'Bắc Giang', 'Thành phố Bắc Giang', 'Phường Hoàng Văn Thụ', 'BG-TP-HVT'),
                ('Việt Nam', 'Bắc Giang', 'Thành phố Bắc Giang', 'Phường Trần Phú', 'BG-TP-TP'),
                ('Việt Nam', 'Bắc Giang', 'Thành phố Bắc Giang', 'Phường Mỹ Độ', 'BG-TP-MD'),
                ('Việt Nam', 'Bắc Giang', 'Thành phố Bắc Giang', 'Phường Lê Lợi', 'BG-TP-LL'),
                ('Việt Nam', 'Bắc Giang', 'Thành phố Bắc Giang', 'Phường Xương Giang', 'BG-TP-XG'),
                ('Việt Nam', 'Bắc Giang', 'Thành phố Bắc Giang', 'Phường Đa Mai', 'BG-TP-DM'),
                ('Việt Nam', 'Bắc Giang', 'Thành phố Bắc Giang', 'Phường Dĩnh Kế', 'BG-TP-DK'),

                -- ==================== NAM ĐỊNH ====================
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Hạ Long', 'ND-TP-HL'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Trần Tế Xương', 'ND-TP-TTX'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Vị Hoàng', 'ND-TP-VH'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Vị Xuyên', 'ND-TP-VX'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Quang Trung', 'ND-TP-QT'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Cửa Bắc', 'ND-TP-CB'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Nguyễn Du', 'ND-TP-ND'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Bà Triệu', 'ND-TP-BT'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Trường Thi', 'ND-TP-TT'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Phan Đình Phùng', 'ND-TP-PDP'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Ngô Quyền', 'ND-TP-NQ'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Trần Hưng Đạo', 'ND-TP-THD'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Trần Đăng Ninh', 'ND-TP-TDN'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Năng Tĩnh', 'ND-TP-NT'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Văn Miếu', 'ND-TP-VM'),
                ('Việt Nam', 'Nam Định', 'Thành phố Nam Định', 'Phường Trần Quang Khải', 'ND-TP-TQK'),

                -- ==================== THÁI BÌNH ====================
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', 'Phường Trần Hưng Đạo', 'TB-TP-THD'),
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', 'Phường Trần Lãm', 'TB-TP-TL'),
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', 'Phường Phú Khánh', 'TB-TP-PK'),
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', 'Phường Tiền Phong', 'TB-TP-TP'),
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', 'Phường Trần Thành Ngọ', 'TB-TP-TTN'),
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', 'Phường Quang Trung', 'TB-TP-QT'),
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', 'Phường Lê Hồng Phong', 'TB-TP-LHP'),
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', 'Phường Bồ Xuyên', 'TB-TP-BX'),
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', 'Phường Đề Thám', 'TB-TP-DT'),
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', 'Phường Kỳ Bá', 'TB-TP-KB'),
                ('Việt Nam', 'Thái Bình', 'Thành phố Thái Bình', 'Phường Hoàng Diệu', 'TB-TP-HD'),

                -- ==================== NINH BÌNH ====================
                ('Việt Nam', 'Ninh Bình', 'Thành phố Ninh Bình', 'Phường Đông Thành', 'NB-TP-DT'),
                ('Việt Nam', 'Ninh Bình', 'Thành phố Ninh Bình', 'Phường Tân Thành', 'NB-TP-TT'),
                ('Việt Nam', 'Ninh Bình', 'Thành phố Ninh Bình', 'Phường Thanh Bình', 'NB-TP-TB'),
                ('Việt Nam', 'Ninh Bình', 'Thành phố Ninh Bình', 'Phường Vân Giang', 'NB-TP-VG'),
                ('Việt Nam', 'Ninh Bình', 'Thành phố Ninh Bình', 'Phường Bích Đào', 'NB-TP-BD'),
                ('Việt Nam', 'Ninh Bình', 'Thành phố Ninh Bình', 'Phường Phúc Thành', 'NB-TP-PT'),
                ('Việt Nam', 'Ninh Bình', 'Thành phố Ninh Bình', 'Phường Nam Bình', 'NB-TP-NB'),
                ('Việt Nam', 'Ninh Bình', 'Thành phố Ninh Bình', 'Phường Nam Thành', 'NB-TP-NT'),
                ('Việt Nam', 'Ninh Bình', 'Thành phố Ninh Bình', 'Phường Ninh Khánh', 'NB-TP-NK'),

                -- ==================== HÀ NAM - Phủ Lý ====================
                ('Việt Nam', 'Hà Nam', 'Phủ Lý', 'Phường Quang Trung', 'HNA-PL-QT'),
                ('Việt Nam', 'Hà Nam', 'Phủ Lý', 'Phường Lương Khánh Thiện', 'HNA-PL-LKT'),
                ('Việt Nam', 'Hà Nam', 'Phủ Lý', 'Phường Lê Hồng Phong', 'HNA-PL-LHP'),
                ('Việt Nam', 'Hà Nam', 'Phủ Lý', 'Phường Minh Khai', 'HNA-PL-MK'),
                ('Việt Nam', 'Hà Nam', 'Phủ Lý', 'Phường Hai Bà Trưng', 'HNA-PL-HBT'),
                ('Việt Nam', 'Hà Nam', 'Phủ Lý', 'Phường Trần Hưng Đạo', 'HNA-PL-THD'),
                ('Việt Nam', 'Hà Nam', 'Phủ Lý', 'Phường Thanh Châu', 'HNA-PL-TC'),
                ('Việt Nam', 'Hà Nam', 'Phủ Lý', 'Phường Trần Phú', 'HNA-PL-TP'),
                ('Việt Nam', 'Hà Nam', 'Phủ Lý', 'Phường Châu Sơn', 'HNA-PL-CS'),

                -- ==================== HƯNG YÊN ====================
                ('Việt Nam', 'Hưng Yên', 'Thành phố Hưng Yên', 'Phường Lam Sơn', 'HY-TP-LS'),
                ('Việt Nam', 'Hưng Yên', 'Thành phố Hưng Yên', 'Phường Hiến Nam', 'HY-TP-HN'),
                ('Việt Nam', 'Hưng Yên', 'Thành phố Hưng Yên', 'Phường An Tảo', 'HY-TP-AT'),
                ('Việt Nam', 'Hưng Yên', 'Thành phố Hưng Yên', 'Phường Lê Lợi', 'HY-TP-LL'),
                ('Việt Nam', 'Hưng Yên', 'Thành phố Hưng Yên', 'Phường Minh Khai', 'HY-TP-MK'),
                ('Việt Nam', 'Hưng Yên', 'Thành phố Hưng Yên', 'Phường Quang Trung', 'HY-TP-QT'),
                ('Việt Nam', 'Hưng Yên', 'Thành phố Hưng Yên', 'Phường Hồng Châu', 'HY-TP-HC'),
                ('Việt Nam', 'Hưng Yên', 'Thành phố Hưng Yên', 'Phường Trung Nghĩa', 'HY-TP-TN'),

                -- ==================== QUẢNG NAM - Tam Kỳ ====================
                ('Việt Nam', 'Quảng Nam', 'Tam Kỳ', 'Phường Tân Thạnh', 'QNM-TK-TT'),
                ('Việt Nam', 'Quảng Nam', 'Tam Kỳ', 'Phường Phước Hòa', 'QNM-TK-PH'),
                ('Việt Nam', 'Quảng Nam', 'Tam Kỳ', 'Phường An Mỹ', 'QNM-TK-AM'),
                ('Việt Nam', 'Quảng Nam', 'Tam Kỳ', 'Phường Hòa Hương', 'QNM-TK-HH'),
                ('Việt Nam', 'Quảng Nam', 'Tam Kỳ', 'Phường An Xuân', 'QNM-TK-AX'),
                ('Việt Nam', 'Quảng Nam', 'Tam Kỳ', 'Phường An Sơn', 'QNM-TK-AS'),
                ('Việt Nam', 'Quảng Nam', 'Tam Kỳ', 'Phường Trường Xuân', 'QNM-TK-TX'),
                ('Việt Nam', 'Quảng Nam', 'Tam Kỳ', 'Phường An Phú', 'QNM-TK-AP'),

                -- ==================== QUẢNG NAM - Hội An ====================
                ('Việt Nam', 'Quảng Nam', 'Hội An', 'Phường Minh An', 'QNM-HA-MA'),
                ('Việt Nam', 'Quảng Nam', 'Hội An', 'Phường Tân An', 'QNM-HA-TA'),
                ('Việt Nam', 'Quảng Nam', 'Hội An', 'Phường Cẩm Phô', 'QNM-HA-CP'),
                ('Việt Nam', 'Quảng Nam', 'Hội An', 'Phường Thanh Hà', 'QNM-HA-TH'),
                ('Việt Nam', 'Quảng Nam', 'Hội An', 'Phường Sơn Phong', 'QNM-HA-SP'),
                ('Việt Nam', 'Quảng Nam', 'Hội An', 'Phường Cẩm Châu', 'QNM-HA-CC'),
                ('Việt Nam', 'Quảng Nam', 'Hội An', 'Phường Cửa Đại', 'QNM-HA-CD'),
                ('Việt Nam', 'Quảng Nam', 'Hội An', 'Phường Cẩm An', 'QNM-HA-CA'),
                ('Việt Nam', 'Quảng Nam', 'Hội An', 'Phường Cẩm Hà', 'QNM-HA-CH'),

                -- ==================== BÌNH ĐỊNH - Quy Nhơn ====================
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Nhơn Bình', 'BD-QN-NB'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Nhơn Phú', 'BD-QN-NP'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Đống Đa', 'BD-QN-DD'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Trần Quang Diệu', 'BD-QN-TQD'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Hải Cảng', 'BD-QN-HC'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Thị Nại', 'BD-QN-TN'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Lê Hồng Phong', 'BD-QN-LHP'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Trần Hưng Đạo', 'BD-QN-THD'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Nguyễn Văn Cừ', 'BD-QN-NVC'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Lê Lợi', 'BD-QN-LL'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Trần Phú', 'BD-QN-TP'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Bùi Thị Xuân', 'BD-QN-BTX'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Ngô Mây', 'BD-QN-NM'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Lý Thường Kiệt', 'BD-QN-LTK'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Ghềnh Ráng', 'BD-QN-GR'),
                ('Việt Nam', 'Bình Định', 'Quy Nhơn', 'Phường Nhơn Châu', 'BD-QN-NC'),

                -- ==================== PHÚ YÊN - Tuy Hòa ====================
                ('Việt Nam', 'Phú Yên', 'Tuy Hòa', 'Phường 1', 'PY-TH-P1'),
                ('Việt Nam', 'Phú Yên', 'Tuy Hòa', 'Phường 2', 'PY-TH-P2'),
                ('Việt Nam', 'Phú Yên', 'Tuy Hòa', 'Phường 3', 'PY-TH-P3'),
                ('Việt Nam', 'Phú Yên', 'Tuy Hòa', 'Phường 4', 'PY-TH-P4'),
                ('Việt Nam', 'Phú Yên', 'Tuy Hòa', 'Phường 5', 'PY-TH-P5'),
                ('Việt Nam', 'Phú Yên', 'Tuy Hòa', 'Phường 6', 'PY-TH-P6'),
                ('Việt Nam', 'Phú Yên', 'Tuy Hòa', 'Phường 7', 'PY-TH-P7'),
                ('Việt Nam', 'Phú Yên', 'Tuy Hòa', 'Phường 8', 'PY-TH-P8'),
                ('Việt Nam', 'Phú Yên', 'Tuy Hòa', 'Phường 9', 'PY-TH-P9'),

                -- ==================== KHÁNH HÒA - Cam Ranh ====================
                ('Việt Nam', 'Khánh Hòa', 'Cam Ranh', 'Phường Cam Nghĩa', 'KH-CR-CN'),
                ('Việt Nam', 'Khánh Hòa', 'Cam Ranh', 'Phường Cam Phúc Bắc', 'KH-CR-CPB'),
                ('Việt Nam', 'Khánh Hòa', 'Cam Ranh', 'Phường Cam Phúc Nam', 'KH-CR-CPN'),
                ('Việt Nam', 'Khánh Hòa', 'Cam Ranh', 'Phường Cam Lộc', 'KH-CR-CL'),
                ('Việt Nam', 'Khánh Hòa', 'Cam Ranh', 'Phường Cam Phú', 'KH-CR-CP'),
                ('Việt Nam', 'Khánh Hòa', 'Cam Ranh', 'Phường Ba Ngòi', 'KH-CR-BN'),
                ('Việt Nam', 'Khánh Hòa', 'Cam Ranh', 'Phường Cam Thuận', 'KH-CR-CT'),
                ('Việt Nam', 'Khánh Hòa', 'Cam Ranh', 'Phường Cam Lợi', 'KH-CR-CLo'),
                ('Việt Nam', 'Khánh Hòa', 'Cam Ranh', 'Phường Cam Linh', 'KH-CR-CLi'),

                -- ==================== BÌNH THUẬN - Phan Thiết ====================
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Đức Nghĩa', 'BT-PT-DN'),
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Đức Thắng', 'BT-PT-DT'),
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Phú Hài', 'BT-PT-PH'),
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Phú Thủy', 'BT-PT-PT'),
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Phú Tài', 'BT-PT-PTa'),
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Phú Trinh', 'BT-PT-PTr'),
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Xuân An', 'BT-PT-XA'),
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Thanh Hải', 'BT-PT-TH'),
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Bình Hưng', 'BT-PT-BH'),
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Hàm Tiến', 'BT-PT-HT'),
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Mũi Né', 'BT-PT-MN'),
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Hưng Long', 'BT-PT-HL'),
                ('Việt Nam', 'Bình Thuận', 'Phan Thiết', 'Phường Đức Long', 'BT-PT-DL'),

                -- ==================== LÂM ĐỒNG - Đà Lạt ====================
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'Phường 1', 'LĐ-DL-P1'),
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'Phường 2', 'LĐ-DL-P2'),
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'Phường 3', 'LĐ-DL-P3'),
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'Phường 4', 'LĐ-DL-P4'),
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'Phường 5', 'LĐ-DL-P5'),
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'Phường 6', 'LĐ-DL-P6'),
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'Phường 7', 'LĐ-DL-P7'),
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'Phường 8', 'LĐ-DL-P8'),
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'Phường 9', 'LĐ-DL-P9'),
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'Phường 10', 'LĐ-DL-P10'),
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'Phường 11', 'LĐ-DL-P11'),
                ('Việt Nam', 'Lâm Đồng', 'Đà Lạt', 'Phường 12', 'LĐ-DL-P12'),

                -- ==================== BÀ RỊA - VŨNG TÀU ====================
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường 1', 'BRVT-VT-P1'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường 2', 'BRVT-VT-P2'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường 3', 'BRVT-VT-P3'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường 4', 'BRVT-VT-P4'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường 5', 'BRVT-VT-P5'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường 7', 'BRVT-VT-P7'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường 8', 'BRVT-VT-P8'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường 9', 'BRVT-VT-P9'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường 10', 'BRVT-VT-P10'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường 11', 'BRVT-VT-P11'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường 12', 'BRVT-VT-P12'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường Thắng Tam', 'BRVT-VT-TT'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường Thắng Nhì', 'BRVT-VT-TN'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường Thắng Nhất', 'BRVT-VT-TNh'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường Rạch Dừa', 'BRVT-VT-RD'),
                ('Việt Nam', 'Bà Rịa - Vũng Tàu', 'Vũng Tàu', 'Phường Nguyễn An Ninh', 'BRVT-VT-NAN'),

                -- ==================== BÌNH DƯƠNG - Dĩ An ====================
                ('Việt Nam', 'Bình Dương', 'Dĩ An', 'Phường Dĩ An', 'BD-DA-DA'),
                ('Việt Nam', 'Bình Dương', 'Dĩ An', 'Phường Tân Bình', 'BD-DA-TB'),
                ('Việt Nam', 'Bình Dương', 'Dĩ An', 'Phường Tân Đông Hiệp', 'BD-DA-TDH'),
                ('Việt Nam', 'Bình Dương', 'Dĩ An', 'Phường Bình An', 'BD-DA-BA'),
                ('Việt Nam', 'Bình Dương', 'Dĩ An', 'Phường Bình Thắng', 'BD-DA-BT'),
                ('Việt Nam', 'Bình Dương', 'Dĩ An', 'Phường Đông Hòa', 'BD-DA-DH'),
                ('Việt Nam', 'Bình Dương', 'Dĩ An', 'Phường An Bình', 'BD-DA-AB'),

                -- ==================== BÌNH DƯƠNG - Thuận An ====================
                ('Việt Nam', 'Bình Dương', 'Thuận An', 'Phường An Thạnh', 'BD-TA-AT'),
                ('Việt Nam', 'Bình Dương', 'Thuận An', 'Phường Lái Thiêu', 'BD-TA-LT'),
                ('Việt Nam', 'Bình Dương', 'Thuận An', 'Phường Bình Chuẩn', 'BD-TA-BC'),
                ('Việt Nam', 'Bình Dương', 'Thuận An', 'Phường Thuận Giao', 'BD-TA-TG'),
                ('Việt Nam', 'Bình Dương', 'Thuận An', 'Phường An Phú', 'BD-TA-AP'),
                ('Việt Nam', 'Bình Dương', 'Thuận An', 'Phường Hưng Định', 'BD-TA-HD'),
                ('Việt Nam', 'Bình Dương', 'Thuận An', 'Phường Bình Nhâm', 'BD-TA-BN'),
                ('Việt Nam', 'Bình Dương', 'Thuận An', 'Phường Bình Hòa', 'BD-TA-BH'),
                ('Việt Nam', 'Bình Dương', 'Thuận An', 'Phường Vĩnh Phú', 'BD-TA-VP'),

                -- ==================== ĐỒNG NAI - Long Khánh ====================
                ('Việt Nam', 'Đồng Nai', 'Long Khánh', 'Phường Xuân Trung', 'DNI-LK-XT'),
                ('Việt Nam', 'Đồng Nai', 'Long Khánh', 'Phường Xuân Thanh', 'DNI-LK-XTh'),
                ('Việt Nam', 'Đồng Nai', 'Long Khánh', 'Phường Xuân Bình', 'DNI-LK-XB'),
                ('Việt Nam', 'Đồng Nai', 'Long Khánh', 'Phường Xuân An', 'DNI-LK-XA'),
                ('Việt Nam', 'Đồng Nai', 'Long Khánh', 'Phường Xuân Hòa', 'DNI-LK-XH'),
                ('Việt Nam', 'Đồng Nai', 'Long Khánh', 'Phường Phú Bình', 'DNI-LK-PB'),
                ('Việt Nam', 'Đồng Nai', 'Long Khánh', 'Phường Suối Tre', 'DNI-LK-ST'),
                ('Việt Nam', 'Đồng Nai', 'Long Khánh', 'Phường Bảo Vinh', 'DNI-LK-BV'),
                ('Việt Nam', 'Đồng Nai', 'Long Khánh', 'Phường Xuân Lập', 'DNI-LK-XL'),

                -- ==================== TIỀN GIANG - Mỹ Tho ====================
                ('Việt Nam', 'Tiền Giang', 'Mỹ Tho', 'Phường 1', 'TG-MT-P1'),
                ('Việt Nam', 'Tiền Giang', 'Mỹ Tho', 'Phường 2', 'TG-MT-P2'),
                ('Việt Nam', 'Tiền Giang', 'Mỹ Tho', 'Phường 3', 'TG-MT-P3'),
                ('Việt Nam', 'Tiền Giang', 'Mỹ Tho', 'Phường 4', 'TG-MT-P4'),
                ('Việt Nam', 'Tiền Giang', 'Mỹ Tho', 'Phường 5', 'TG-MT-P5'),
                ('Việt Nam', 'Tiền Giang', 'Mỹ Tho', 'Phường 6', 'TG-MT-P6'),
                ('Việt Nam', 'Tiền Giang', 'Mỹ Tho', 'Phường 7', 'TG-MT-P7'),
                ('Việt Nam', 'Tiền Giang', 'Mỹ Tho', 'Phường 8', 'TG-MT-P8'),
                ('Việt Nam', 'Tiền Giang', 'Mỹ Tho', 'Phường 9', 'TG-MT-P9'),
                ('Việt Nam', 'Tiền Giang', 'Mỹ Tho', 'Phường 10', 'TG-MT-P10'),

                -- ==================== AN GIANG - Long Xuyên ====================
                ('Việt Nam', 'An Giang', 'Long Xuyên', 'Phường Mỹ Bình', 'AG-LX-MB'),
                ('Việt Nam', 'An Giang', 'Long Xuyên', 'Phường Mỹ Long', 'AG-LX-ML'),
                ('Việt Nam', 'An Giang', 'Long Xuyên', 'Phường Đông Xuyên', 'AG-LX-DX'),
                ('Việt Nam', 'An Giang', 'Long Xuyên', 'Phường Mỹ Thới', 'AG-LX-MT'),
                ('Việt Nam', 'An Giang', 'Long Xuyên', 'Phường Mỹ Thạnh', 'AG-LX-MTh'),
                ('Việt Nam', 'An Giang', 'Long Xuyên', 'Phường Mỹ Hòa', 'AG-LX-MH'),
                ('Việt Nam', 'An Giang', 'Long Xuyên', 'Phường Mỹ Phước', 'AG-LX-MP'),
                ('Việt Nam', 'An Giang', 'Long Xuyên', 'Phường Mỹ Quý', 'AG-LX-MQ'),
                ('Việt Nam', 'An Giang', 'Long Xuyên', 'Phường Bình Đức', 'AG-LX-BD'),
                ('Việt Nam', 'An Giang', 'Long Xuyên', 'Phường Bình Khánh', 'AG-LX-BK'),

                -- ==================== KIÊN GIANG - Rạch Giá ====================
                ('Việt Nam', 'Kiên Giang', 'Rạch Giá', 'Phường Vĩnh Thanh Vân', 'KG-RG-VTV'),
                ('Việt Nam', 'Kiên Giang', 'Rạch Giá', 'Phường Vĩnh Thanh', 'KG-RG-VT'),
                ('Việt Nam', 'Kiên Giang', 'Rạch Giá', 'Phường Vĩnh Quang', 'KG-RG-VQ'),
                ('Việt Nam', 'Kiên Giang', 'Rạch Giá', 'Phường Vĩnh Lạc', 'KG-RG-VL'),
                ('Việt Nam', 'Kiên Giang', 'Rạch Giá', 'Phường Vĩnh Bảo', 'KG-RG-VB'),
                ('Việt Nam', 'Kiên Giang', 'Rạch Giá', 'Phường Vĩnh Lợi', 'KG-RG-VLo'),
                ('Việt Nam', 'Kiên Giang', 'Rạch Giá', 'Phường An Bình', 'KG-RG-AB'),
                ('Việt Nam', 'Kiên Giang', 'Rạch Giá', 'Phường An Hòa', 'KG-RG-AH'),
                ('Việt Nam', 'Kiên Giang', 'Rạch Giá', 'Phường Rạch Sỏi', 'KG-RG-RS'),
                ('Việt Nam', 'Kiên Giang', 'Rạch Giá', 'Phường Vĩnh Hiệp', 'KG-RG-VH');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""Locations"" WHERE ""Ward"" IS NOT NULL 
                AND (""Code"" LIKE '%QN-MC-%' OR ""Code"" LIKE '%QN-CP-%' OR ""Code"" LIKE '%QN-UB-%'
                OR ""Code"" LIKE '%BG-TP-%' OR ""Code"" LIKE '%ND-TP-%' OR ""Code"" LIKE '%TB-TP-%'
                OR ""Code"" LIKE '%NB-TP-%' OR ""Code"" LIKE '%HNA-PL-%' OR ""Code"" LIKE '%HY-TP-%'
                OR ""Code"" LIKE '%QNM-TK-%' OR ""Code"" LIKE '%QNM-HA-%' OR ""Code"" LIKE '%BD-QN-%'
                OR ""Code"" LIKE '%PY-TH-%' OR ""Code"" LIKE '%KH-CR-%' OR ""Code"" LIKE '%BT-PT-%'
                OR ""Code"" LIKE '%LĐ-DL-%' OR ""Code"" LIKE '%BRVT-VT-%' OR ""Code"" LIKE '%BD-DA-%'
                OR ""Code"" LIKE '%BD-TA-%' OR ""Code"" LIKE '%DNI-LK-%' OR ""Code"" LIKE '%TG-MT-%'
                OR ""Code"" LIKE '%AG-LX-%' OR ""Code"" LIKE '%KG-RG-%');");
        }
    }
}
