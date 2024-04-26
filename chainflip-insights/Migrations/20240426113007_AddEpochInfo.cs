using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainflipInsights.Migrations
{
    /// <inheritdoc />
    public partial class AddEpochInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "epoch_info",
                columns: table => new
                {
                    EpochId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EpochStart = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    MinimumBond = table.Column<double>(type: "double", nullable: false),
                    TotalBond = table.Column<double>(type: "double", nullable: false),
                    MaxBid = table.Column<double>(type: "double", nullable: false),
                    TotalRewards = table.Column<double>(type: "double", nullable: false),
                    ExplorerUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_epoch_info", x => x.EpochId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_epoch_info_EpochStart",
                table: "epoch_info",
                column: "EpochStart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "epoch_info");
        }
    }
}
