using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iyaspark.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FloorNumber",
                table: "Tenants");

            migrationBuilder.AddColumn<string>(
                name: "FloorCode",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FloorCode",
                table: "Tenants");

            migrationBuilder.AddColumn<int>(
                name: "FloorNumber",
                table: "Tenants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
