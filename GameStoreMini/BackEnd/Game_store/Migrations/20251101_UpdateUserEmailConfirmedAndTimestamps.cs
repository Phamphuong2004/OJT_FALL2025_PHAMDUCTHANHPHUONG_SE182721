using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEmailConfirmedAndTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Set EmailConfirmed true for existing users that are not confirmed
            migrationBuilder.Sql(
                "UPDATE \"Users\" SET \"EmailConfirmed\" = true WHERE COALESCE(\"EmailConfirmed\", false) = false;"
            );

            // Ensure CreatedAt has a sensible value for existing rows
            migrationBuilder.Sql(
                "UPDATE \"Users\" SET \"CreatedAt\" = now() WHERE \"CreatedAt\" IS NULL OR \"CreatedAt\" = '0001-01-01';"
            );

            // Set UpdatedAt to now() where null
            migrationBuilder.Sql(
                "UPDATE \"Users\" SET \"UpdatedAt\" = now() WHERE \"UpdatedAt\" IS NULL;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert EmailConfirmed back to false (best-effort)
            migrationBuilder.Sql(
                "UPDATE \"Users\" SET \"EmailConfirmed\" = false WHERE true;"
            );

            // Revert UpdatedAt and CreatedAt is not reversible reliably; leave as-is.
        }
    }
}
