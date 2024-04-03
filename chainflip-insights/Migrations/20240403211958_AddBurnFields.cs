using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainflipInsights.Migrations
{
    /// <inheritdoc />
    public partial class AddBurnFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "BurnAmount",
                table: "burn_info",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddColumn<uint>(
                name: "BurnBlock",
                table: "burn_info",
                type: "int unsigned",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<string>(
                name: "BurnHash",
                table: "burn_info",
                type: "varchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BurnBlock",
                table: "burn_info");

            migrationBuilder.DropColumn(
                name: "BurnHash",
                table: "burn_info");

            migrationBuilder.AlterColumn<decimal>(
                name: "BurnAmount",
                table: "burn_info",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }
    }
}
