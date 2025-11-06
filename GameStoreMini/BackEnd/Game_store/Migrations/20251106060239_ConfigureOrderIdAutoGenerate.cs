using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game_store.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureOrderIdAutoGenerate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create sequences for Orders and OrderItems if they don't exist
            migrationBuilder.Sql(@"
                CREATE SEQUENCE IF NOT EXISTS ""Orders_Id_seq"";
                ALTER TABLE ""Orders"" ALTER COLUMN ""Id"" SET DEFAULT nextval('""Orders_Id_seq""');
                ALTER SEQUENCE ""Orders_Id_seq"" OWNED BY ""Orders"".""Id"";
                SELECT setval('""Orders_Id_seq""', COALESCE((SELECT MAX(""Id"") FROM ""Orders""), 0) + 1, false);
            ");

            migrationBuilder.Sql(@"
                CREATE SEQUENCE IF NOT EXISTS ""OrderItems_Id_seq"";
                ALTER TABLE ""OrderItems"" ALTER COLUMN ""Id"" SET DEFAULT nextval('""OrderItems_Id_seq""');
                ALTER SEQUENCE ""OrderItems_Id_seq"" OWNED BY ""OrderItems"".""Id"";
                SELECT setval('""OrderItems_Id_seq""', COALESCE((SELECT MAX(""Id"") FROM ""OrderItems""), 0) + 1, false);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Orders"" ALTER COLUMN ""Id"" DROP DEFAULT;
                DROP SEQUENCE IF EXISTS ""Orders_Id_seq"";
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""OrderItems"" ALTER COLUMN ""Id"" DROP DEFAULT;
                DROP SEQUENCE IF EXISTS ""OrderItems_Id_seq"";
            ");
        }
    }
}
