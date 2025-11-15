using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class SeedComplete63ProvincesWardsCommunes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Part 1: Bổ sung thêm các quận/huyện còn lại của Hà Nội, TP.HCM và các tỉnh thành lớn
            migrationBuilder.Sql(@"
                INSERT INTO ""Locations"" (""Country"", ""City"", ""District"", ""Ward"", ""Code"") VALUES
                
                -- ==================== HÀ NỘI - Quận Hà Đông ====================
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Nguyễn Trãi', 'HN-HDO-NT'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Mộ Lao', 'HN-HDO-ML'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Văn Quán', 'HN-HDO-VQ'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Vĩnh Hưng', 'HN-HDO-VH'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Yên Nghĩa', 'HN-HDO-YN'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Kiến Hưng', 'HN-HDO-KH'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Phú Lãm', 'HN-HDO-PL'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Phú Lương', 'HN-HDO-PLU'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Dương Nội', 'HN-HDO-DN'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Đồng Mai', 'HN-HDO-DM'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Biên Giang', 'HN-HDO-BG'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Yết Kiêu', 'HN-HDO-YK'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Quang Trung', 'HN-HDO-QT'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường La Khê', 'HN-HDO-LK'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Phúc La', 'HN-HDO-PLA'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Hà Cầu', 'HN-HDO-HC'),
                ('Việt Nam', 'Hà Nội', 'Hà Đông', 'Phường Văn Phú', 'HN-HDO-VP'),

                -- ==================== HỒ CHÍ MINH - Quận 9 (Thành phố Thủ Đức) ====================
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Long Bình', 'HCM-Q9-LB'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Long Thạnh Mỹ', 'HCM-Q9-LTM'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Tân Phú', 'HCM-Q9-TP'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Hiệp Phú', 'HCM-Q9-HP'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Tăng Nhơn Phú A', 'HCM-Q9-TNPA'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Tăng Nhơn Phú B', 'HCM-Q9-TNPB'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Phước Long A', 'HCM-Q9-PLA'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Phước Long B', 'HCM-Q9-PLB'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Trường Thạnh', 'HCM-Q9-TT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Long Phước', 'HCM-Q9-LP'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Long Trường', 'HCM-Q9-LTR'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Phước Bình', 'HCM-Q9-PB'),
                ('Việt Nam', 'Hồ Chí Minh', 'Quận 9', 'Phường Phú Hữu', 'HCM-Q9-PH'),

                -- ==================== HỒ CHÍ MINH - Thành phố Thủ Đức ====================
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'Phường Linh Xuân', 'HCM-TD-LX'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'Phường Bình Chiểu', 'HCM-TD-BC'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'Phường Linh Trung', 'HCM-TD-LT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'Phường Tam Bình', 'HCM-TD-TB'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'Phường Tam Phú', 'HCM-TD-TP'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'Phường Hiệp Bình Phước', 'HCM-TD-HBP'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'Phường Hiệp Bình Chánh', 'HCM-TD-HBC'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'Phường Linh Chiểu', 'HCM-TD-LC'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'Phường Linh Tây', 'HCM-TD-LTA'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'Phường Linh Đông', 'HCM-TD-LD'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'Phường Bình Thọ', 'HCM-TD-BT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Thủ Đức', 'Phường Trường Thọ', 'HCM-TD-TT'),

                -- ==================== HỒ CHÍ MINH - Quận Bình Tân ====================
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Tân', 'Phường Bình Hưng Hòa', 'HCM-BTN-BHH'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Tân', 'Phường Bình Hưng Hòa A', 'HCM-BTN-BHHA'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Tân', 'Phường Bình Hưng Hòa B', 'HCM-BTN-BHHB'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Tân', 'Phường Bình Trị Đông', 'HCM-BTN-BTĐ'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Tân', 'Phường Bình Trị Đông A', 'HCM-BTN-BTĐA'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Tân', 'Phường Bình Trị Đông B', 'HCM-BTN-BTĐB'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Tân', 'Phường Tân Tạo', 'HCM-BTN-TT'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Tân', 'Phường Tân Tạo A', 'HCM-BTN-TTA'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Tân', 'Phường An Lạc', 'HCM-BTN-AL'),
                ('Việt Nam', 'Hồ Chí Minh', 'Bình Tân', 'Phường An Lạc A', 'HCM-BTN-ALA'),

                -- ==================== ĐÀ NẴNG - Quận Liên Chiểu ====================
                ('Việt Nam', 'Đà Nẵng', 'Liên Chiểu', 'Phường Hòa Hiệp Bắc', 'DN-LC-HHB'),
                ('Việt Nam', 'Đà Nẵng', 'Liên Chiểu', 'Phường Hòa Hiệp Nam', 'DN-LC-HHN'),
                ('Việt Nam', 'Đà Nẵng', 'Liên Chiểu', 'Phường Hòa Khánh Bắc', 'DN-LC-HKB'),
                ('Việt Nam', 'Đà Nẵng', 'Liên Chiểu', 'Phường Hòa Khánh Nam', 'DN-LC-HKN'),
                ('Việt Nam', 'Đà Nẵng', 'Liên Chiểu', 'Phường Hòa Minh', 'DN-LC-HM'),

                -- ==================== ĐÀ NẴNG - Quận Cẩm Lệ ====================
                ('Việt Nam', 'Đà Nẵng', 'Cẩm Lệ', 'Phường Khuê Trung', 'DN-CL-KT'),
                ('Việt Nam', 'Đà Nẵng', 'Cẩm Lệ', 'Phường Hòa Phát', 'DN-CL-HP'),
                ('Việt Nam', 'Đà Nẵng', 'Cẩm Lệ', 'Phường Hòa An', 'DN-CL-HA'),
                ('Việt Nam', 'Đà Nẵng', 'Cẩm Lệ', 'Phường Hòa Thọ Tây', 'DN-CL-HTT'),
                ('Việt Nam', 'Đà Nẵng', 'Cẩm Lệ', 'Phường Hòa Thọ Đông', 'DN-CL-HTĐ'),

                -- ==================== HẢI PHÒNG - Quận Ngô Quyền ====================
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'Phường Máy Chai', 'HP-NQ-MC'),
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'Phường Máy Tơ', 'HP-NQ-MT'),
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'Phường Vạn Mỹ', 'HP-NQ-VM'),
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'Phường Cầu Tre', 'HP-NQ-CT'),
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'Phường Lạc Viên', 'HP-NQ-LV'),
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'Phường Gia Viên', 'HP-NQ-GV'),
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'Phường Đông Khê', 'HP-NQ-ĐK'),
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'Phường Cầu Đất', 'HP-NQ-CĐ'),
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'Phường Lê Lợi', 'HP-NQ-LL'),
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'Phường Đằng Giang', 'HP-NQ-ĐG'),
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'Phường Lạch Tray', 'HP-NQ-LT'),
                ('Việt Nam', 'Hải Phòng', 'Ngô Quyền', 'Phường Đổng Quốc Bình', 'HP-NQ-ĐQB'),

                -- ==================== HẢI PHÒNG - Quận Kiến An ====================
                ('Việt Nam', 'Hải Phòng', 'Kiến An', 'Phường Quán Trữ', 'HP-KA-QT'),
                ('Việt Nam', 'Hải Phòng', 'Kiến An', 'Phường Lãm Hà', 'HP-KA-LH'),
                ('Việt Nam', 'Hải Phòng', 'Kiến An', 'Phường Đồng Hòa', 'HP-KA-ĐH'),
                ('Việt Nam', 'Hải Phòng', 'Kiến An', 'Phường Bắc Sơn', 'HP-KA-BS'),
                ('Việt Nam', 'Hải Phòng', 'Kiến An', 'Phường Nam Sơn', 'HP-KA-NS'),
                ('Việt Nam', 'Hải Phòng', 'Kiến An', 'Phường Ngọc Sơn', 'HP-KA-NGS'),
                ('Việt Nam', 'Hải Phòng', 'Kiến An', 'Phường Trần Thành Ngọ', 'HP-KA-TTN'),
                ('Việt Nam', 'Hải Phòng', 'Kiến An', 'Phường Văn Đẩu', 'HP-KA-VĐ'),
                ('Việt Nam', 'Hải Phòng', 'Kiến An', 'Phường Phù Liễn', 'HP-KA-PL'),
                ('Việt Nam', 'Hải Phòng', 'Kiến An', 'Phường Tràng Minh', 'HP-KA-TM'),

                -- ==================== CẦN THƠ - Quận Cái Răng ====================
                ('Việt Nam', 'Cần Thơ', 'Cái Răng', 'Phường Lê Bình', 'CT-CR-LB'),
                ('Việt Nam', 'Cần Thơ', 'Cái Răng', 'Phường Hưng Phú', 'CT-CR-HP'),
                ('Việt Nam', 'Cần Thơ', 'Cái Răng', 'Phường Hưng Thạnh', 'CT-CR-HT'),
                ('Việt Nam', 'Cần Thơ', 'Cái Răng', 'Phường Ba Láng', 'CT-CR-BL'),
                ('Việt Nam', 'Cần Thơ', 'Cái Răng', 'Phường Thường Thạnh', 'CT-CR-TT'),
                ('Việt Nam', 'Cần Thơ', 'Cái Răng', 'Phường Phú Thứ', 'CT-CR-PT'),
                ('Việt Nam', 'Cần Thơ', 'Cái Răng', 'Phường Tân Phú', 'CT-CR-TP'),

                -- ==================== CẦN THƠ - Quận Bình Thủy ====================
                ('Việt Nam', 'Cần Thơ', 'Bình Thủy', 'Phường Bình Thủy', 'CT-BT-BT'),
                ('Việt Nam', 'Cần Thơ', 'Bình Thủy', 'Phường Trà An', 'CT-BT-TA'),
                ('Việt Nam', 'Cần Thơ', 'Bình Thủy', 'Phường Trà Nóc', 'CT-BT-TN'),
                ('Việt Nam', 'Cần Thơ', 'Bình Thủy', 'Phường Thới An Đông', 'CT-BT-TAĐ'),
                ('Việt Nam', 'Cần Thơ', 'Bình Thủy', 'Phường An Thới', 'CT-BT-AT'),
                ('Việt Nam', 'Cần Thơ', 'Bình Thủy', 'Phường Bùi Hữu Nghĩa', 'CT-BT-BHN'),
                ('Việt Nam', 'Cần Thơ', 'Bình Thủy', 'Phường Long Hòa', 'CT-BT-LH'),
                ('Việt Nam', 'Cần Thơ', 'Bình Thủy', 'Phường Long Tuyền', 'CT-BT-LT'),

                -- ==================== TỈNH KHÁC - BẮC NINH ====================
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Vũ Ninh', 'BN-TP-VN'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Đáp Cầu', 'BN-TP-DC'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Thị Cầu', 'BN-TP-TC'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Kinh Bắc', 'BN-TP-KB'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Vệ An', 'BN-TP-VA'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Tiền An', 'BN-TP-TA'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Đại Phúc', 'BN-TP-DP'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Ninh Xá', 'BN-TP-NX'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Suối Hoa', 'BN-TP-SH'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Võ Cường', 'BN-TP-VC'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Hòa Long', 'BN-TP-HL'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Vạn An', 'BN-TP-VAN'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Khúc Xuyên', 'BN-TP-KX'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Phong Khê', 'BN-TP-PK'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Kim Chân', 'BN-TP-KC'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Năm Sơn', 'BN-TP-NS'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Khắc Niệm', 'BN-TP-KN'),
                ('Việt Nam', 'Bắc Ninh', 'Thành phố Bắc Ninh', 'Phường Hạp Lĩnh', 'BN-TP-HaL'),

                -- ==================== HẢI DƯƠNG ====================
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Cẩm Thượng', 'HD-TP-CT'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Bình Hàn', 'HD-TP-BH'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Ngọc Châu', 'HD-TP-NC'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Nhị Châu', 'HD-TP-NCH'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Quang Trung', 'HD-TP-QT'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Nguyễn Trãi', 'HD-TP-NT'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Phạm Ngũ Lão', 'HD-TP-PNL'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Trần Hưng Đạo', 'HD-TP-THD'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Trần Phú', 'HD-TP-TP'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Thanh Bình', 'HD-TP-TB'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Tân Bình', 'HD-TP-TanB'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Lê Thanh Nghị', 'HD-TP-LTN'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Hải Tân', 'HD-TP-HT'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Tứ Minh', 'HD-TP-TM'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Việt Hòa', 'HD-TP-VH'),
                ('Việt Nam', 'Hải Dương', 'Thành phố Hải Dương', 'Phường Ái Quốc', 'HD-TP-AQ'),

                -- ==================== VĨNH PHÚC - Vĩnh Yên ====================
                ('Việt Nam', 'Vĩnh Phúc', 'Vĩnh Yên', 'Phường Tích Sơn', 'VP-VY-TS'),
                ('Việt Nam', 'Vĩnh Phúc', 'Vĩnh Yên', 'Phường Liên Bảo', 'VP-VY-LB'),
                ('Việt Nam', 'Vĩnh Phúc', 'Vĩnh Yên', 'Phường Hội Hợp', 'VP-VY-HH'),
                ('Việt Nam', 'Vĩnh Phúc', 'Vĩnh Yên', 'Phường Đống Đa', 'VP-VY-DD'),
                ('Việt Nam', 'Vĩnh Phúc', 'Vĩnh Yên', 'Phường Ngô Quyền', 'VP-VY-NQ'),
                ('Việt Nam', 'Vĩnh Phúc', 'Vĩnh Yên', 'Phường Đồng Tâm', 'VP-VY-DT'),
                ('Việt Nam', 'Vĩnh Phúc', 'Vĩnh Yên', 'Phường Định Trung', 'VP-VY-DTR'),
                ('Việt Nam', 'Vĩnh Phúc', 'Vĩnh Yên', 'Phường Khai Quang', 'VP-VY-KQ'),

                -- ==================== PHÚ THỌ - Việt Trì ====================
                ('Việt Nam', 'Phú Thọ', 'Việt Trì', 'Phường Dữu Lâu', 'PT-VT-DL'),
                ('Việt Nam', 'Phú Thọ', 'Việt Trì', 'Phường Vân Cơ', 'PT-VT-VC'),
                ('Việt Nam', 'Phú Thọ', 'Việt Trì', 'Phường Nông Trang', 'PT-VT-NT'),
                ('Việt Nam', 'Phú Thọ', 'Việt Trì', 'Phường Tân Dân', 'PT-VT-TD'),
                ('Việt Nam', 'Phú Thọ', 'Việt Trì', 'Phường Gia Cẩm', 'PT-VT-GC'),
                ('Việt Nam', 'Phú Thọ', 'Việt Trì', 'Phường Tiên Cát', 'PT-VT-TC'),
                ('Việt Nam', 'Phú Thọ', 'Việt Trì', 'Phường Thọ Sơn', 'PT-VT-TS'),
                ('Việt Nam', 'Phú Thọ', 'Việt Trì', 'Phường Thanh Miếu', 'PT-VT-TM'),
                ('Việt Nam', 'Phú Thọ', 'Việt Trì', 'Phường Bạch Hạc', 'PT-VT-BH'),
                ('Việt Nam', 'Phú Thọ', 'Việt Trì', 'Phường Bến Gót', 'PT-VT-BG'),
                ('Việt Nam', 'Phú Thọ', 'Việt Trì', 'Phường Vân Phú', 'PT-VT-VP'),

                -- ==================== THANH HÓA ====================
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Hàm Rồng', 'TH-TP-HR'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Đông Thọ', 'TH-TP-DT'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Nam Ngạn', 'TH-TP-NN'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Trường Thi', 'TH-TP-TT'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Điện Biên', 'TH-TP-DB'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Phú Sơn', 'TH-TP-PS'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Lam Sơn', 'TH-TP-LS'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Ba Đình', 'TH-TP-BD'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Ngọc Trạo', 'TH-TP-NT'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Đông Vệ', 'TH-TP-DV'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Đông Sơn', 'TH-TP-DS'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Tân Sơn', 'TH-TP-TS'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Đông Cương', 'TH-TP-DC'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Đông Hương', 'TH-TP-DH'),
                ('Việt Nam', 'Thanh Hóa', 'Thành phố Thanh Hóa', 'Phường Đông Hải', 'TH-TP-DHai'),

                -- ==================== NGHỆ AN - Vinh ====================
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Đông Vĩnh', 'NA-V-DV'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Hà Huy Tập', 'NA-V-HHT'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Lê Lợi', 'NA-V-LL'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Quán Bàu', 'NA-V-QB'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Hưng Bình', 'NA-V-HB'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Hưng Phúc', 'NA-V-HP'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Hưng Dũng', 'NA-V-HD'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Cửa Nam', 'NA-V-CN'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Bến Thủy', 'NA-V-BT'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Hồng Sơn', 'NA-V-HS'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Trường Thi', 'NA-V-TT'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Đội Cung', 'NA-V-DC'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Lê Mao', 'NA-V-LM'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Trung Đô', 'NA-V-TD'),
                ('Việt Nam', 'Nghệ An', 'Vinh', 'Phường Vinh Tân', 'NA-V-VT'),

                -- ==================== THỪA THIÊN HUẾ - Huế ====================
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Phú Thuận', 'HUE-TP-PT'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Phú Bình', 'HUE-TP-PB'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Phú Hiệp', 'HUE-TP-PH'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Phú Hậu', 'HUE-TP-PHa'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Thuận Hòa', 'HUE-TP-TH'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Thuận Lộc', 'HUE-TP-TL'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Phú Hòa', 'HUE-TP-PHo'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Phú Cát', 'HUE-TP-PC'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Kim Long', 'HUE-TP-KL'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Vĩ Dạ', 'HUE-TP-VD'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Phường Đúc', 'HUE-TP-PD'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Vĩnh Ninh', 'HUE-TP-VN'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Phú Hội', 'HUE-TP-PHoi'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Phú Nhuận', 'HUE-TP-PN'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Xuân Phú', 'HUE-TP-XP'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Trường An', 'HUE-TP-TA'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Phước Vĩnh', 'HUE-TP-PV'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường An Cựu', 'HUE-TP-AC'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường An Hòa', 'HUE-TP-AH'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Hương Sơ', 'HUE-TP-HS'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Thuỷ Biều', 'HUE-TP-TB'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Hương Long', 'HUE-TP-HL'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường Thuỷ Xuân', 'HUE-TP-TX'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường An Đông', 'HUE-TP-AD'),
                ('Việt Nam', 'Thừa Thiên Huế', 'Huế', 'Phường An Tây', 'HUE-TP-AT');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""Locations"" WHERE ""Ward"" IS NOT NULL 
                AND (""Code"" LIKE '%HN-HDO-%' OR ""Code"" LIKE '%HCM-Q9-%' OR ""Code"" LIKE '%HCM-TD-%' 
                OR ""Code"" LIKE '%HCM-BTN-%' OR ""Code"" LIKE '%DN-LC-%' OR ""Code"" LIKE '%DN-CL-%'
                OR ""Code"" LIKE '%HP-NQ-%' OR ""Code"" LIKE '%HP-KA-%' OR ""Code"" LIKE '%CT-CR-%'
                OR ""Code"" LIKE '%CT-BT-%' OR ""Code"" LIKE '%BN-TP-%' OR ""Code"" LIKE '%HD-TP-%'
                OR ""Code"" LIKE '%VP-VY-%' OR ""Code"" LIKE '%PT-VT-%' OR ""Code"" LIKE '%TH-TP-%'
                OR ""Code"" LIKE '%NA-V-%' OR ""Code"" LIKE '%HUE-TP-%');");
        }
    }
}
