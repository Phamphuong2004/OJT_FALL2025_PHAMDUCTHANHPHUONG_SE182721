using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    public partial class AddWardToLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ward",
                table: "Locations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ward",
                table: "Locations");
        }
    }
}
