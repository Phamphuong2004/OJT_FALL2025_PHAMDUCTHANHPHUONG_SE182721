using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationModelToDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "City", "Code", "Country", "District" },
                values: new object[,]
                {
                    { 1, "Hà Nội", "HN", "Việt Nam", null },
                    { 2, "Hồ Chí Minh", "HCM", "Việt Nam", null },
                    { 3, "Đà Nẵng", "DN", "Việt Nam", null }
                });
        }
    }
}
