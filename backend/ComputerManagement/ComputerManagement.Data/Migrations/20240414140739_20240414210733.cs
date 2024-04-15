using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20240414210733 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "computer_history",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ComputerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_computer_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_computer_history_computer_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "computer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_computer_history_ComputerId",
                table: "computer_history",
                column: "ComputerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "computer_history");
        }
    }
}
