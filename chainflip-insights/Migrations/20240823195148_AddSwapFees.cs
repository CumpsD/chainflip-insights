using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainflipInsights.Migrations
{
    /// <inheritdoc />
    public partial class AddSwapFees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AllFeesUsd",
                table: "swap_info",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AllUserFeesUsd",
                table: "swap_info",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BurnUsd",
                table: "swap_info",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LiquidityFeesUsd",
                table: "swap_info",
                type: "double",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllFeesUsd",
                table: "swap_info");

            migrationBuilder.DropColumn(
                name: "AllUserFeesUsd",
                table: "swap_info");

            migrationBuilder.DropColumn(
                name: "BurnUsd",
                table: "swap_info");

            migrationBuilder.DropColumn(
                name: "LiquidityFeesUsd",
                table: "swap_info");
        }
    }
}
