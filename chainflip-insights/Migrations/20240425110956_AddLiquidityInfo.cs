using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainflipInsights.Migrations
{
    /// <inheritdoc />
    public partial class AddLiquidityInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "liquidity_info",
                columns: table => new
                {
                    LiquidityId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LiquidityDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    AmountUsd = table.Column<double>(type: "double", nullable: false),
                    Asset = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExplorerUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_liquidity_info", x => x.LiquidityId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_liquidity_info_Asset",
                table: "liquidity_info",
                column: "Asset");

            migrationBuilder.CreateIndex(
                name: "IX_liquidity_info_LiquidityDate",
                table: "liquidity_info",
                column: "LiquidityDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "liquidity_info");
        }
    }
}
