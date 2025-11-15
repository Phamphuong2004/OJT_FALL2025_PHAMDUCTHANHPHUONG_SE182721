using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExistingOrdersWithOrderNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update existing orders that don't have OrderNumber
            migrationBuilder.Sql(@"
                UPDATE ""Orders""
                SET ""OrderNumber"" = 'ORD-' || 
                    TO_CHAR(""CreatedAt"", 'YYYYMMDDHH24MISS') || '-' || 
                    SUBSTRING(MD5(RANDOM()::text || ""Id""::text) FROM 1 FOR 6)
                WHERE ""OrderNumber"" IS NULL OR ""OrderNumber"" = '';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No need to revert, OrderNumber can stay
        }
    }
}
