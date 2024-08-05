using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainflipInsights.Migrations
{
    /// <inheritdoc />
    public partial class AddBoostedSwaps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BoostFeeBps",
                table: "swap_info",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BoostFeeUsd",
                table: "swap_info",
                type: "double",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoostFeeBps",
                table: "swap_info");

            migrationBuilder.DropColumn(
                name: "BoostFeeUsd",
                table: "swap_info");
        }
    }
}
