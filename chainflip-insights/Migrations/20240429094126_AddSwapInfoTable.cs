using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainflipInsights.Migrations
{
    /// <inheritdoc />
    public partial class AddSwapInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "swap_info",
                columns: table => new
                {
                    SwapId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SwapDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    DepositAmount = table.Column<double>(type: "double", nullable: false),
                    DepositValueUsd = table.Column<double>(type: "double", nullable: false),
                    SourceAsset = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EgressAmount = table.Column<double>(type: "double", nullable: false),
                    EgressValueUsd = table.Column<double>(type: "double", nullable: false),
                    DestinationAsset = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeltaUsd = table.Column<double>(type: "double", nullable: false),
                    DeltaUsdPercentage = table.Column<double>(type: "double", nullable: false),
                    Broker = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExplorerUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_swap_info", x => x.SwapId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_swap_info_SwapDate",
                table: "swap_info",
                column: "SwapDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "swap_info");
        }
    }
}
