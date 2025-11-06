using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed Categories - Danh mục game
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, "Hành động", "hanh-dong" },
                    { 2, "Phiêu lưu", "phieu-luu" },
                    { 3, "Nhập vai", "nhap-vai" },
                    { 4, "Chiến thuật", "chien-thuat" },
                    { 5, "Thể thao", "the-thao" },
                    { 6, "Mô phỏng", "mo-phong" },
                    { 7, "Đua xe", "dua-xe" },
                    { 8, "Đối kháng", "doi-khang" },
                    { 9, "Kinh dị", "kinh-di" },
                    { 10, "Độc lập", "doc-lap" }
                });

            // Seed Games - Danh sách game
            migrationBuilder.Sql(@"
                INSERT INTO ""Games"" (""Title"", ""Description"", ""Price"", ""Stock"", ""ImageUrl"", ""SortOrder"", ""CreatedAt"") VALUES
                ('Cyberpunk 2077', 'Game phiêu lưu hành động thế giới mở tại thành phố Night City', 1399000, 100, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1091500/header.jpg', 1, NOW()),
                ('The Witcher 3: Thợ Săn Quái Vật', 'Game nhập vai thế giới mở với câu chuyện hấp dẫn trong vũ trụ giả tưởng', 899000, 150, 'https://cdn.cloudflare.steamstatic.com/steam/apps/292030/header.jpg', 2, NOW()),
                ('Red Dead Redemption 2', 'Câu chuyện sử thi về cuộc sống ở Mỹ vào buổi bình minh của thời hiện đại', 1399000, 80, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1174180/header.jpg', 3, NOW()),
                ('Grand Theft Auto V', 'Game phiêu lưu thế giới mở tại Los Santos', 699000, 200, 'https://cdn.cloudflare.steamstatic.com/steam/apps/271590/header.jpg', 4, NOW()),
                ('Elden Ring', 'Game nhập vai hành động từ FromSoftware và Bandai Namco', 1399000, 120, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1245620/header.jpg', 5, NOW()),
                ('Minecraft', 'Game sandbox với khả năng sáng tạo vô tận', 629000, 500, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1086940/header.jpg', 6, NOW()),
                ('Baldur''s Gate 3', 'Game nhập vai sử thi từ Larian Studios', 1399000, 90, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1086940/header.jpg', 7, NOW()),
                ('God of War', 'Game hành động phiêu lưu với nhân vật Kratos', 1199000, 110, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1593500/header.jpg', 8, NOW()),
                ('Hogwarts Legacy', 'Game nhập vai hành động thế giới mở trong vũ trụ Harry Potter', 1399000, 95, 'https://cdn.cloudflare.steamstatic.com/steam/apps/990080/header.jpg', 9, NOW()),
                ('Starfield', 'Game nhập vai khám phá không gian từ Bethesda', 1599000, 75, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1716740/header.jpg', 10, NOW()),
                ('FIFA 24', 'Game mô phỏng bóng đá mới nhất', 1399000, 130, 'https://cdn.cloudflare.steamstatic.com/steam/apps/2195250/header.jpg', 11, NOW()),
                ('Call of Duty: Modern Warfare III', 'Game bắn súng góc nhìn thứ nhất', 1599000, 140, 'https://cdn.cloudflare.steamstatic.com/steam/apps/2519060/header.jpg', 12, NOW()),
                ('Resident Evil 4 Remake', 'Game kinh dị sinh tồn remake', 1399000, 85, 'https://cdn.cloudflare.steamstatic.com/steam/apps/2050650/header.jpg', 13, NOW()),
                ('Forza Horizon 5', 'Game đua xe thế giới mở tại Mexico', 1399000, 100, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1551360/header.jpg', 14, NOW()),
                ('Stardew Valley', 'Game mô phỏng nông trại nhập vai', 349000, 300, 'https://cdn.cloudflare.steamstatic.com/steam/apps/413150/header.jpg', 15, NOW()),
                ('Hades', 'Game dungeon crawler theo phong cách rogue-like', 599000, 200, 'https://cdn.cloudflare.steamstatic.com/steam/apps/1145360/header.jpg', 16, NOW()),
                ('Assassin''s Creed Valhalla', 'Game hành động phiêu lưu chủ đề Viking', 1399000, 90, 'https://cdn.cloudflare.steamstatic.com/steam/apps/2208920/header.jpg', 17, NOW()),
                ('Dark Souls III', 'Game nhập vai hành động đầy thử thách', 899000, 120, 'https://cdn.cloudflare.steamstatic.com/steam/apps/374320/header.jpg', 18, NOW()),
                ('Sekiro: Shadows Die Twice', 'Game hành động phiêu lưu từ FromSoftware', 1399000, 95, 'https://cdn.cloudflare.steamstatic.com/steam/apps/814380/header.jpg', 19, NOW()),
                ('Monster Hunter: World', 'Game nhập vai hành động về săn bắn quái vật', 699000, 150, 'https://cdn.cloudflare.steamstatic.com/steam/apps/582010/header.jpg', 20, NOW());
            ");

            // Seed GameCategories - Liên kết Game với Danh mục (Many-to-Many)
            migrationBuilder.Sql(@"
                INSERT INTO ""GameCategories"" (""GameId"", ""CategoryId"") VALUES
                -- Cyberpunk 2077: Hành động, Phiêu lưu, Nhập vai
                (1, 1), (1, 2), (1, 3),
                -- The Witcher 3: Phiêu lưu, Nhập vai
                (2, 2), (2, 3),
                -- Red Dead Redemption 2: Hành động, Phiêu lưu
                (3, 1), (3, 2),
                -- GTA V: Hành động, Phiêu lưu
                (4, 1), (4, 2),
                -- Elden Ring: Hành động, Nhập vai
                (5, 1), (5, 3),
                -- Minecraft: Phiêu lưu, Mô phỏng, Độc lập
                (6, 2), (6, 6), (6, 10),
                -- Baldur's Gate 3: Nhập vai, Chiến thuật
                (7, 3), (7, 4),
                -- God of War: Hành động, Phiêu lưu
                (8, 1), (8, 2),
                -- Hogwarts Legacy: Hành động, Phiêu lưu, Nhập vai
                (9, 1), (9, 2), (9, 3),
                -- Starfield: Phiêu lưu, Nhập vai, Mô phỏng
                (10, 2), (10, 3), (10, 6),
                -- FIFA 24: Thể thao
                (11, 5),
                -- Call of Duty: Hành động
                (12, 1),
                -- Resident Evil 4: Hành động, Kinh dị
                (13, 1), (13, 9),
                -- Forza Horizon 5: Đua xe, Thể thao
                (14, 7), (14, 5),
                -- Stardew Valley: Mô phỏng, Độc lập
                (15, 6), (15, 10),
                -- Hades: Hành động, Nhập vai, Độc lập
                (16, 1), (16, 3), (16, 10),
                -- Assassin's Creed Valhalla: Hành động, Phiêu lưu
                (17, 1), (17, 2),
                -- Dark Souls III: Hành động, Nhập vai
                (18, 1), (18, 3),
                -- Sekiro: Hành động, Phiêu lưu
                (19, 1), (19, 2),
                -- Monster Hunter World: Hành động, Nhập vai
                (20, 1), (20, 3);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove seeded data
            migrationBuilder.Sql(@"DELETE FROM ""GameCategories"";");
            migrationBuilder.Sql(@"DELETE FROM ""Games"";");
            migrationBuilder.DeleteData(table: "Categories", keyColumn: "Id", keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }
    }
}
