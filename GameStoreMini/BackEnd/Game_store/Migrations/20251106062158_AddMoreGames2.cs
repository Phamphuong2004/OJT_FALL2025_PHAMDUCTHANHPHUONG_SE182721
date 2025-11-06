using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreGames2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm 15 game mới
            migrationBuilder.Sql(@"
                INSERT INTO ""Games"" (""Title"", ""Description"", ""Price"", ""Stock"", ""ImageUrl"", ""SortOrder"", ""CreatedAt"") VALUES
                ('Palworld', 'Game sinh tồn thế giới mở với yếu tố thu thập quái vật', 499000, 200, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1623730/header.jpg', 21, NOW()),
                ('Tekken 8', 'Game đối kháng đối đầu mới nhất trong series Tekken', 1399000, 100, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1778820/header.jpg', 22, NOW()),
                ('Persona 5 Royal', 'Game nhập vai Nhật Bản với phong cách nghệ thuật độc đáo', 1199000, 85, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1687950/header.jpg', 23, NOW()),
                ('Street Fighter 6', 'Game đối kháng kinh điển với đồ họa hiện đại', 1399000, 90, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1364780/header.jpg', 24, NOW()),
                ('Sons of The Forest', 'Game sinh tồn kinh dị trong rừng sâu', 699000, 150, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1326470/header.jpg', 25, NOW()),
                ('Terraria', 'Game sandbox 2D với khám phá và xây dựng vô tận', 199000, 400, 'https://cdn.cloudflare.steamstatic.com/steam/apps/105600/header.jpg', 26, NOW()),
                ('Dead Space Remake', 'Game kinh dị sinh tồn trong không gian', 1399000, 80, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1693980/header.jpg', 27, NOW()),
                ('Mortal Kombat 11', 'Game đối kháng bạo lực với fatality nổi tiếng', 899000, 110, 'https://cdn.cloudflare.steamstatic.com/steam/apps/976310/header.jpg', 28, NOW()),
                ('Cities: Skylines II', 'Game mô phỏng xây dựng và quản lý thành phố', 1199000, 120, 'https://cdn.cloudflare.steamstatic.com/steam/apps/949230/header.jpg', 29, NOW()),
                ('Age of Empires IV', 'Game chiến thuật thời gian thực lịch sử', 1099000, 100, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1466860/header.jpg', 30, NOW()),
                ('Diablo IV', 'Game nhập vai hành động trong thế giới tối tăm', 1599000, 130, 'https://cdn.cloudflare.steamstatic.com/steam/apps/2344520/header.jpg', 31, NOW()),
                ('Final Fantasy XVI', 'Game nhập vai hành động với câu chuyện hoành tráng', 1599000, 75, 'https://cdn.cloudflare.steamstatic.com/steam/apps/2515020/header.jpg', 32, NOW()),
                ('Valorant', 'Game bắn súng chiến thuật 5v5', 0, 999, 'https://images.contentstack.io/v3/assets/bltb6530b271fddd0b1/blt5c6c94c2e7ec428e/valorant-header.jpg', 33, NOW()),
                ('League of Legends', 'Game MOBA 5v5 phổ biến nhất thế giới', 0, 999, 'https://images.contentstack.io/v3/assets/blt731023b3d1c33c3c/lol-header.jpg', 34, NOW()),
                ('Counter-Strike 2', 'Game bắn súng chiến thuật kinh điển', 0, 999, 'https://cdn.cloudflare.steamstatic.com/steam/apps/730/header.jpg', 35, NOW());
            ");

            // Thêm liên kết GameCategories
            migrationBuilder.Sql(@"
                INSERT INTO ""GameCategories"" (""GameId"", ""CategoryId"") VALUES
                -- Palworld: Phiêu lưu, Mô phỏng, Độc lập
                (21, 2), (21, 6), (21, 10),
                -- Tekken 8: Đối kháng
                (22, 8),
                -- Persona 5 Royal: Nhập vai
                (23, 3),
                -- Street Fighter 6: Đối kháng
                (24, 8),
                -- Sons of The Forest: Phiêu lưu, Kinh dị
                (25, 2), (25, 9),
                -- Terraria: Phiêu lưu, Độc lập
                (26, 2), (26, 10),
                -- Dead Space: Hành động, Kinh dị
                (27, 1), (27, 9),
                -- Mortal Kombat 11: Đối kháng
                (28, 8),
                -- Cities Skylines II: Chiến thuật, Mô phỏng
                (29, 4), (29, 6),
                -- Age of Empires IV: Chiến thuật
                (30, 4),
                -- Diablo IV: Hành động, Nhập vai
                (31, 1), (31, 3),
                -- Final Fantasy XVI: Hành động, Nhập vai
                (32, 1), (32, 3),
                -- Valorant: Hành động
                (33, 1),
                -- League of Legends: Chiến thuật
                (34, 4),
                -- Counter-Strike 2: Hành động
                (35, 1);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa data khi rollback
            migrationBuilder.Sql(@"DELETE FROM ""GameCategories"" WHERE ""GameId"" >= 21 AND ""GameId"" <= 35;");
            migrationBuilder.Sql(@"DELETE FROM ""Games"" WHERE ""Id"" >= 21 AND ""Id"" <= 35;");
        }
    }
}
