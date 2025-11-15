using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreVietnamWardsComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO ""Locations"" (""Country"", ""City"", ""District"", ""Ward"", ""Code"") VALUES
                -- HÀ NỘI - Quận Hoàng Mai
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Hoàng Văn Thụ', 'HN-HM-HVT'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Giáp Bát', 'HN-HM-GB'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Lĩnh Nam', 'HN-HM-LN'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Thịnh Liệt', 'HN-HM-TL'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Trần Phú', 'HN-HM-TP'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Hoàng Liệt', 'HN-HM-HL'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Yên Sở', 'HN-HM-YS'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Tân Mai', 'HN-HM-TM'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Đại Kim', 'HN-HM-DK'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Tương Mai', 'HN-HM-TMA'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Định Công', 'HN-HM-DC'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Mai Động', 'HN-HM-MD'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Thanh Trì', 'HN-HM-TT'),
                ('Việt Nam', 'Hà Nội', 'Hoàng Mai', 'Phường Vĩnh Hưng', 'HN-HM-VH'),

                -- HÀ NỘI - Quận Thanh Xuân
                ('Việt Nam', 'Hà Nội', 'Thanh Xuân', 'Phường Nhân Chính', 'HN-TX-NC'),
                ('Việt Nam', 'Hà Nội', 'Thanh Xuân', 'Phường Thượng Đình', 'HN-TX-TD'),
                ('Việt Nam', 'Hà Nội', 'Thanh Xuân', 'Phường Khương Trung', 'HN-TX-KT'),
                ('Việt Nam', 'Hà Nội', 'Thanh Xuân', 'Phường Khương Mai', 'HN-TX-KM'),
                ('Việt Nam', 'Hà Nội', 'Thanh Xuân', 'Phường Thanh Xuân Bắc', 'HN-TX-TXB'),
                ('Việt Nam', 'Hà Nội', 'Thanh Xuân', 'Phường Thanh Xuân Trung', 'HN-TX-TXT'),
                ('Việt Nam', 'Hà Nội', 'Thanh Xuân', 'Phường Thanh Xuân Nam', 'HN-TX-TXN'),
                ('Việt Nam', 'Hà Nội', 'Thanh Xuân', 'Phường Kim Giang', 'HN-TX-KG'),
                ('Việt Nam', 'Hà Nội', 'Thanh Xuân', 'Phường Hạ Đình', 'HN-TX-HD'),
                ('Việt Nam', 'Hà Nội', 'Thanh Xuân', 'Phường Khương Đình', 'HN-TX-KD'),
                ('Việt Nam', 'Hà Nội', 'Thanh Xuân', 'Phường Phương Liệt', 'HN-TX-PL'),

                -- HÀ NỘI - Quận Long Biên
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Thượng Thanh', 'HN-LB-TT'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Ngọc Thúy', 'HN-LB-NT'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Giang Biên', 'HN-LB-GB'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Đức Giang', 'HN-LB-DG'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Việt Hưng', 'HN-LB-VH'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Gia Thụy', 'HN-LB-GT'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Ngọc Lâm', 'HN-LB-NL'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Phúc Lợi', 'HN-LB-PL'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Bồ Đề', 'HN-LB-BD'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Sài Đồng', 'HN-LB-SD'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Long Biên', 'HN-LB-LB'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Thạch Bàn', 'HN-LB-TB'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Phúc Đồng', 'HN-LB-PD'),
                ('Việt Nam', 'Hà Nội', 'Long Biên', 'Phường Cự Khối', 'HN-LB-CK'),

                -- HÀ NỘI - Quận Tây Hồ
                ('Việt Nam', 'Hà Nội', 'Tây Hồ', 'Phường Phú Thượng', 'HN-TH-PT'),
                ('Việt Nam', 'Hà Nội', 'Tây Hồ', 'Phường Nhật Tân', 'HN-TH-NT'),
                ('Việt Nam', 'Hà Nội', 'Tây Hồ', 'Phường Tứ Liên', 'HN-TH-TL'),
                ('Việt Nam', 'Hà Nội', 'Tây Hồ', 'Phường Quảng An', 'HN-TH-QA'),
                ('Việt Nam', 'Hà Nội', 'Tây Hồ', 'Phường Xuân La', 'HN-TH-XL'),
                ('Việt Nam', 'Hà Nội', 'Tây Hồ', 'Phường Yên Phụ', 'HN-TH-YP'),
                ('Việt Nam', 'Hà Nội', 'Tây Hồ', 'Phường Bưởi', 'HN-TH-BU'),
                ('Việt Nam', 'Hà Nội', 'Tây Hồ', 'Phường Thụy Khuê', 'HN-TH-TK'),

                -- HÀ NỘI - Quận Nam Từ Liêm
                ('Việt Nam', 'Hà Nội', 'Nam Từ Liêm', 'Phường Cầu Diễn', 'HN-NTL-CD'),
                ('Việt Nam', 'Hà Nội', 'Nam Từ Liêm', 'Phường Xuân Phương', 'HN-NTL-XP'),
                ('Việt Nam', 'Hà Nội', 'Nam Từ Liêm', 'Phường Phương Canh', 'HN-NTL-PC'),
                ('Việt Nam', 'Hà Nội', 'Nam Từ Liêm', 'Phường Mỹ Đình 1', 'HN-NTL-MD1'),
                ('Việt Nam', 'Hà Nội', 'Nam Từ Liêm', 'Phường Mỹ Đình 2', 'HN-NTL-MD2'),
                ('Việt Nam', 'Hà Nội', 'Nam Từ Liêm', 'Phường Tây Mỗ', 'HN-NTL-TM'),
                ('Việt Nam', 'Hà Nội', 'Nam Từ Liêm', 'Phường Mễ Trì', 'HN-NTL-MT'),
                ('Việt Nam', 'Hà Nội', 'Nam Từ Liêm', 'Phường Phú Đô', 'HN-NTL-PD'),
                ('Việt Nam', 'Hà Nội', 'Nam Từ Liêm', 'Phường Đại Mỗ', 'HN-NTL-DM'),
                ('Việt Nam', 'Hà Nội', 'Nam Từ Liêm', 'Phường Trung Văn', 'HN-NTL-TV'),

                -- HÀ NỘI - Quận Bắc Từ Liêm
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Thượng Cát', 'HN-BTL-TC'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Liên Mạc', 'HN-BTL-LM'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Đông Ngạc', 'HN-BTL-DN'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Đức Thắng', 'HN-BTL-DT'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Thụy Phương', 'HN-BTL-TP'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Tây Tựu', 'HN-BTL-TT'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Xuân Đỉnh', 'HN-BTL-XD'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Xuân Tảo', 'HN-BTL-XT'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Minh Khai', 'HN-BTL-MK'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Cổ Nhuế 1', 'HN-BTL-CN1'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Cổ Nhuế 2', 'HN-BTL-CN2'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Phú Diễn', 'HN-BTL-PD'),
                ('Việt Nam', 'Hà Nội', 'Bắc Từ Liêm', 'Phường Phúc Diễn', 'HN-BTL-PDE'),

                -- HỒ CHÍ MINH - Quận 2 (Thủ Đức)
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 2', 'Phường Thảo Điền', 'HCM-Q2-TĐ'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 2', 'Phường An Phú', 'HCM-Q2-AP'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 2', 'Phường An Khánh', 'HCM-Q2-AK'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 2', 'Phường Bình An', 'HCM-Q2-BA'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 2', 'Phường Bình Trưng Đông', 'HCM-Q2-BTĐ'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 2', 'Phường Bình Trưng Tây', 'HCM-Q2-BTT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 2', 'Phường Cát Lái', 'HCM-Q2-CL'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 2', 'Phường Thạnh Mỹ Lợi', 'HCM-Q2-TML'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 2', 'Phường Thủ Thiêm', 'HCM-Q2-TT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 2', 'Phường An Lợi Đông', 'HCM-Q2-ALĐ'),

                -- HỒ CHÍ MINH - Quận 4
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 01', 'HCM-Q4-P01'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 02', 'HCM-Q4-P02'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 03', 'HCM-Q4-P03'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 04', 'HCM-Q4-P04'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 05', 'HCM-Q4-P05'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 06', 'HCM-Q4-P06'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 08', 'HCM-Q4-P08'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 09', 'HCM-Q4-P09'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 10', 'HCM-Q4-P10'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 13', 'HCM-Q4-P13'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 14', 'HCM-Q4-P14'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 15', 'HCM-Q4-P15'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 16', 'HCM-Q4-P16'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 4', 'Phường 18', 'HCM-Q4-P18'),

                -- HỒ CHÍ MINH - Quận 6
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 01', 'HCM-Q6-P01'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 02', 'HCM-Q6-P02'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 03', 'HCM-Q6-P03'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 04', 'HCM-Q6-P04'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 05', 'HCM-Q6-P05'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 06', 'HCM-Q6-P06'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 07', 'HCM-Q6-P07'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 08', 'HCM-Q6-P08'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 09', 'HCM-Q6-P09'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 10', 'HCM-Q6-P10'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 11', 'HCM-Q6-P11'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 12', 'HCM-Q6-P12'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 13', 'HCM-Q6-P13'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 6', 'Phường 14', 'HCM-Q6-P14'),

                -- HỒ CHÍ MINH - Quận 7
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 7', 'Phường Tân Thuận Đông', 'HCM-Q7-TTĐ'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 7', 'Phường Tân Thuận Tây', 'HCM-Q7-TTT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 7', 'Phường Tân Kiểng', 'HCM-Q7-TK'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 7', 'Phường Tân Hưng', 'HCM-Q7-TH'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 7', 'Phường Bình Thuận', 'HCM-Q7-BT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 7', 'Phường Tân Quy', 'HCM-Q7-TQ'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 7', 'Phường Phú Thuận', 'HCM-Q7-PT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 7', 'Phường Tân Phú', 'HCM-Q7-TP'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 7', 'Phường Tân Phong', 'HCM-Q7-TPH'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 7', 'Phường Phú Mỹ', 'HCM-Q7-PM'),

                -- HỒ CHÍ MINH - Quận 8
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 01', 'HCM-Q8-P01'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 02', 'HCM-Q8-P02'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 03', 'HCM-Q8-P03'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 04', 'HCM-Q8-P04'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 05', 'HCM-Q8-P05'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 06', 'HCM-Q8-P06'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 07', 'HCM-Q8-P07'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 08', 'HCM-Q8-P08'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 09', 'HCM-Q8-P09'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 10', 'HCM-Q8-P10'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 11', 'HCM-Q8-P11'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 12', 'HCM-Q8-P12'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 13', 'HCM-Q8-P13'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 14', 'HCM-Q8-P14'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 15', 'HCM-Q8-P15'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 8', 'Phường 16', 'HCM-Q8-P16'),

                -- HỒ CHÍ MINH - Quận 10
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 01', 'HCM-Q10-P01'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 02', 'HCM-Q10-P02'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 03', 'HCM-Q10-P03'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 04', 'HCM-Q10-P04'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 05', 'HCM-Q10-P05'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 06', 'HCM-Q10-P06'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 07', 'HCM-Q10-P07'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 08', 'HCM-Q10-P08'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 09', 'HCM-Q10-P09'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 10', 'HCM-Q10-P10'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 11', 'HCM-Q10-P11'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 12', 'HCM-Q10-P12'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 13', 'HCM-Q10-P13'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 14', 'HCM-Q10-P14'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 10', 'Phường 15', 'HCM-Q10-P15'),

                -- HỒ CHÍ MINH - Quận 11
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 01', 'HCM-Q11-P01'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 02', 'HCM-Q11-P02'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 03', 'HCM-Q11-P03'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 04', 'HCM-Q11-P04'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 05', 'HCM-Q11-P05'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 06', 'HCM-Q11-P06'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 07', 'HCM-Q11-P07'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 08', 'HCM-Q11-P08'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 09', 'HCM-Q11-P09'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 10', 'HCM-Q11-P10'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 11', 'HCM-Q11-P11'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 12', 'HCM-Q11-P12'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 13', 'HCM-Q11-P13'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 14', 'HCM-Q11-P14'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 15', 'HCM-Q11-P15'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 11', 'Phường 16', 'HCM-Q11-P16'),

                -- HỒ CHÍ MINH - Quận 12
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 12', 'Phường Thạnh Xuân', 'HCM-Q12-TX'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 12', 'Phường Thạnh Lộc', 'HCM-Q12-TL'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 12', 'Phường Hiệp Thành', 'HCM-Q12-HT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 12', 'Phường Thới An', 'HCM-Q12-TA'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 12', 'Phường Tân Chánh Hiệp', 'HCM-Q12-TCH'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 12', 'Phường An Phú Đông', 'HCM-Q12-APĐ'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 12', 'Phường Tân Thới Hiệp', 'HCM-Q12-TTH'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 12', 'Phường Trung Mỹ Tây', 'HCM-Q12-TMT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 12', 'Phường Tân Hưng Thuận', 'HCM-Q12-THT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 12', 'Phường Đông Hưng Thuận', 'HCM-Q12-ĐHT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 12', 'Phường Tân Thới Nhất', 'HCM-Q12-TTN'),

                -- HỒ CHÍ MINH - Quận Gò Vấp
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 01', 'HCM-GV-P01'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 03', 'HCM-GV-P03'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 04', 'HCM-GV-P04'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 05', 'HCM-GV-P05'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 06', 'HCM-GV-P06'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 07', 'HCM-GV-P07'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 08', 'HCM-GV-P08'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 09', 'HCM-GV-P09'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 10', 'HCM-GV-P10'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 11', 'HCM-GV-P11'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 12', 'HCM-GV-P12'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 13', 'HCM-GV-P13'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 14', 'HCM-GV-P14'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 15', 'HCM-GV-P15'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 16', 'HCM-GV-P16'),
                ('Việt Nam', 'Hồ Chí Minh', 'Gò Vấp', 'Phường 17', 'HCM-GV-P17'),

                -- HỒ CHÍ MINH - Quận Bình Thạnh
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 01', 'HCM-BT-P01'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 02', 'HCM-BT-P02'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 03', 'HCM-BT-P03'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 05', 'HCM-BT-P05'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 06', 'HCM-BT-P06'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 07', 'HCM-BT-P07'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 11', 'HCM-BT-P11'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 12', 'HCM-BT-P12'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 13', 'HCM-BT-P13'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 14', 'HCM-BT-P14'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 15', 'HCM-BT-P15'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 17', 'HCM-BT-P17'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 19', 'HCM-BT-P19'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 21', 'HCM-BT-P21'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 22', 'HCM-BT-P22'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 24', 'HCM-BT-P24'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 25', 'HCM-BT-P25'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 26', 'HCM-BT-P26'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 27', 'HCM-BT-P27'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Thạnh', 'Phường 28', 'HCM-BT-P28'),

                -- HỒ CHÍ MINH - Quận Tân Bình
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 01', 'HCM-TB-P01'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 02', 'HCM-TB-P02'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 03', 'HCM-TB-P03'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 04', 'HCM-TB-P04'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 05', 'HCM-TB-P05'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 06', 'HCM-TB-P06'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 07', 'HCM-TB-P07'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 08', 'HCM-TB-P08'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 09', 'HCM-TB-P09'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 10', 'HCM-TB-P10'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 11', 'HCM-TB-P11'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 12', 'HCM-TB-P12'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 13', 'HCM-TB-P13'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 14', 'HCM-TB-P14'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Bình', 'Phường 15', 'HCM-TB-P15'),

                -- HỒ CHÍ MINH - Quận Tân Phú
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Phú', 'Phường Tân Sơn Nhì', 'HCM-TP-TSN'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Phú', 'Phường Tây Thạnh', 'HCM-TP-TT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Phú', 'Phường Sơn Kỳ', 'HCM-TP-SK'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Phú', 'Phường Tân Quý', 'HCM-TP-TQ'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Phú', 'Phường Tân Thành', 'HCM-TP-TTH'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Phú', 'Phường Phú Thọ Hòa', 'HCM-TP-PTH'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Phú', 'Phường Phú Thạnh', 'HCM-TP-PT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Phú', 'Phường Phú Trung', 'HCM-TP-PTR'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Phú', 'Phường Hòa Thạnh', 'HCM-TP-HT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Phú', 'Phường Hiệp Tân', 'HCM-TP-HTA'),
                ('Việt Nam', 'Hồ Chí Minh', 'Tân Phú', 'Phường Tân Thới Hòa', 'HCM-TP-TTH2'),

                -- HỒ CHÍ MINH - Quận Phú Nhuận
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 01', 'HCM-PN-P01'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 02', 'HCM-PN-P02'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 03', 'HCM-PN-P03'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 04', 'HCM-PN-P04'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 05', 'HCM-PN-P05'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 07', 'HCM-PN-P07'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 08', 'HCM-PN-P08'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 09', 'HCM-PN-P09'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 10', 'HCM-PN-P10'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 11', 'HCM-PN-P11'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 12', 'HCM-PN-P12'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 13', 'HCM-PN-P13'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 15', 'HCM-PN-P15'),
                ('Việt Nam', 'Hồ Chí Minh', 'Phú Nhuận', 'Phường 17', 'HCM-PN-P17'),

                -- ĐÀ NẴNG - Quận Sơn Trà
                ('Việt Nam', 'Đà Nẵng', 'Sơn Trà', 'Phường Thọ Quang', 'DN-ST-TQ'),
                ('Việt Nam', 'Đà Nẵng', 'Sơn Trà', 'Phường Nại Hiên Đông', 'DN-ST-NHĐ'),
                ('Việt Nam', 'Đà Nẵng', 'Sơn Trà', 'Phường Mân Thái', 'DN-ST-MT'),
                ('Việt Nam', 'Đà Nẵng', 'Sơn Trà', 'Phường An Hải Bắc', 'DN-ST-AHB'),
                ('Việt Nam', 'Đà Nẵng', 'Sơn Trà', 'Phường Phước Mỹ', 'DN-ST-PM'),
                ('Việt Nam', 'Đà Nẵng', 'Sơn Trà', 'Phường An Hải Tây', 'DN-ST-AHT'),
                ('Việt Nam', 'Đà Nẵng', 'Sơn Trà', 'Phường An Hải Đông', 'DN-ST-AHĐ'),

                -- ĐÀ NẴNG - Quận Ngũ Hành Sơn
                ('Việt Nam', 'Đà Nẵng', 'Ngũ Hành Sơn', 'Phường Mỹ An', 'DN-NHS-MA'),
                ('Việt Nam', 'Đà Nẵng', 'Ngũ Hành Sơn', 'Phường Khuê Mỹ', 'DN-NHS-KM'),
                ('Việt Nam', 'Đà Nẵng', 'Ngũ Hành Sơn', 'Phường Hòa Quý', 'DN-NHS-HQ'),
                ('Việt Nam', 'Đà Nẵng', 'Ngũ Hành Sơn', 'Phường Hòa Hải', 'DN-NHS-HH'),

                -- HẢI PHÒNG - Quận Lê Chân
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Cát Dài', 'HP-LC-CD'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường An Biên', 'HP-LC-AB'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Lam Sơn', 'HP-LC-LS'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường An Dương', 'HP-LC-AD'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Trần Nguyên Hãn', 'HP-LC-TNH'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Hồ Nam', 'HP-LC-HN'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Trại Cau', 'HP-LC-TC'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Dư Hàng', 'HP-LC-DH'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Hàng Kênh', 'HP-LC-HK'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Đông Hải', 'HP-LC-ĐH'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Niệm Nghĩa', 'HP-LC-NN'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Nghĩa Xá', 'HP-LC-NX'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Dư Hàng Kênh', 'HP-LC-DHK'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Kênh Dương', 'HP-LC-KD'),
                ('Việt Nam', 'Hải Phòng', 'Lê Chân', 'Phường Vĩnh Niệm', 'HP-LC-VN');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""Locations"" WHERE ""Ward"" IS NOT NULL AND ""Code"" LIKE '%HN-HM-%' 
                OR ""Code"" LIKE '%HN-TX-%' OR ""Code"" LIKE '%HN-LB-%' OR ""Code"" LIKE '%HN-TH-%'
                OR ""Code"" LIKE '%HN-NTL-%' OR ""Code"" LIKE '%HN-BTL-%'
                OR ""Code"" LIKE '%HCM-Q2-%' OR ""Code"" LIKE '%HCM-Q4-%' OR ""Code"" LIKE '%HCM-Q6-%'
                OR ""Code"" LIKE '%HCM-Q7-%' OR ""Code"" LIKE '%HCM-Q8-%' OR ""Code"" LIKE '%HCM-Q10-%'
                OR ""Code"" LIKE '%HCM-Q11-%' OR ""Code"" LIKE '%HCM-Q12-%' OR ""Code"" LIKE '%HCM-GV-%'
                OR ""Code"" LIKE '%HCM-BT-%' OR ""Code"" LIKE '%HCM-TB-%' OR ""Code"" LIKE '%HCM-TP-%'
                OR ""Code"" LIKE '%HCM-PN-%' OR ""Code"" LIKE '%DN-ST-%' OR ""Code"" LIKE '%DN-NHS-%'
                OR ""Code"" LIKE '%HP-LC-%';");
        }
    }
}
