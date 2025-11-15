using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class SeedAllWardsVietnam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ward",
                table: "Locations",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ward",
                table: "Locations");
        }
    }
}
