using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iyaspark.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyTypeToGuarantee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyType",
                table: "Guarantees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "Guarantees");
        }
    }
}
