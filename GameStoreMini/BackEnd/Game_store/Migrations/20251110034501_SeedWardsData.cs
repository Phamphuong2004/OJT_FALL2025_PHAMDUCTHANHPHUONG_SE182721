using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    public partial class SeedWardsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // HỒ CHÍ MINH - QUẬN 1 (10 phường)
            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Country", "City", "District", "Ward" },
                values: new object[,]
                {
                    { "Vietnam", "Hồ Chí Minh", "Quận 1", "Phường Bến Nghé" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 1", "Phường Bến Thành" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 1", "Phường Cầu Kho" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 1", "Phường Cầu Ông Lãnh" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 1", "Phường Cô Giang" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 1", "Phường Đa Kao" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 1", "Phường Nguyễn Cư Trinh" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 1", "Phường Nguyễn Thái Bình" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 1", "Phường Phạm Ngũ Lão" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 1", "Phường Tân Định" },

                    // QUẬN 2 (11 phường)
                    { "Vietnam", "Hồ Chí Minh", "Quận 2", "Phường An Khánh" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 2", "Phường An Lợi Đông" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 2", "Phường An Phú" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 2", "Phường Bình An" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 2", "Phường Bình Khánh" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 2", "Phường Bình Trưng Đông" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 2", "Phường Bình Trưng Tây" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 2", "Phường Cát Lái" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 2", "Phường Thạnh Mỹ Lợi" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 2", "Phường Thảo Điền" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 2", "Phường Thủ Thiêm" },

                    // QUẬN 3 (14 phường)
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 01" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 02" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 03" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 04" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 05" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 06" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 07" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 08" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 09" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 10" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 11" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 12" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 13" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 3", "Phường 14" },

                    // QUẬN 4 (16 phường)
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 01" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 02" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 03" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 04" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 05" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 06" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 08" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 09" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 10" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 12" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 13" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 14" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 15" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 16" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 17" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 4", "Phường 18" },

                    // QUẬN 5 (15 phường)
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 01" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 02" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 03" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 04" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 05" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 06" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 07" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 08" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 09" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 10" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 11" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 12" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 13" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 14" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 5", "Phường 15" },

                    // QUẬN 6 (14 phường)
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 01" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 02" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 03" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 04" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 05" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 06" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 07" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 08" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 09" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 10" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 11" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 12" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 13" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 6", "Phường 14" },

                    // QUẬN 7 (10 phường)
                    { "Vietnam", "Hồ Chí Minh", "Quận 7", "Phường Bình Thuận" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 7", "Phường Phú Mỹ" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 7", "Phường Phú Thuận" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 7", "Phường Tân Hưng" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 7", "Phường Tân Kiểng" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 7", "Phường Tân Phong" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 7", "Phường Tân Phú" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 7", "Phường Tân Quy" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 7", "Phường Tân Thuận Đông" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 7", "Phường Tân Thuận Tây" },

                    // QUẬN 8 (16 phường)
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 01" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 02" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 03" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 04" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 05" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 06" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 07" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 08" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 09" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 10" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 11" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 12" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 13" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 14" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 15" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 8", "Phường 16" },

                    // QUẬN 10 (15 phường)
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 01" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 02" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 04" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 05" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 06" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 07" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 08" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 09" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 10" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 11" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 12" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 13" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 14" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 10", "Phường 15" },

                    // QUẬN 11 (16 phường)
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 01" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 02" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 03" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 04" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 05" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 06" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 07" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 08" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 09" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 10" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 11" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 12" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 13" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 14" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 15" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 11", "Phường 16" },

                    // QUẬN 12 (13 phường)
                    { "Vietnam", "Hồ Chí Minh", "Quận 12", "Phường An Phú Đông" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 12", "Phường Đông Hưng Thuận" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 12", "Phường Hiệp Thành" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 12", "Phường Tân Chánh Hiệp" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 12", "Phường Tân Hưng Thuận" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 12", "Phường Tân Thới Hiệp" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 12", "Phường Tân Thới Nhất" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 12", "Phường Thạnh Lộc" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 12", "Phường Thạnh Xuân" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 12", "Phường Thới An" },
                    { "Vietnam", "Hồ Chí Minh", "Quận 12", "Phường Trung Mỹ Tây" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Locations WHERE Ward IS NOT NULL");
        }
    }
}
