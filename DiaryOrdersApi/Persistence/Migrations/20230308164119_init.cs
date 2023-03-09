using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrdersApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiaryOrders",
                columns: table => new
                {
                    DiaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentItem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeelingScore = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryOrders", x => x.DiaryId);
                });

            migrationBuilder.CreateTable(
                name: "DiaryOrderDetails",
                columns: table => new
                {
                    DiaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeneratedDiaryId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryOrderDetails", x => new { x.DiaryId, x.GeneratedDiaryId });
                    table.ForeignKey(
                        name: "FK_DiaryOrderDetails_DiaryOrders_DiaryId",
                        column: x => x.DiaryId,
                        principalTable: "DiaryOrders",
                        principalColumn: "DiaryId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiaryOrderDetails");

            migrationBuilder.DropTable(
                name: "DiaryOrders");
        }
    }
}
