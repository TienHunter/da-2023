using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20240527220125 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_computer_history_computer_ComputerId",
                table: "computer_history");

            migrationBuilder.DropIndex(
                name: "IX_computer_history_ComputerId",
                table: "computer_history");

            migrationBuilder.AddColumn<Guid>(
                name: "ComputerRoomId",
                table: "computer_history",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "computer_history",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "MonitorSessionId",
                table: "computer_history",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "computer_history",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComputerRoomId",
                table: "computer_history");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "computer_history");

            migrationBuilder.DropColumn(
                name: "MonitorSessionId",
                table: "computer_history");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "computer_history");

            migrationBuilder.CreateIndex(
                name: "IX_computer_history_ComputerId",
                table: "computer_history",
                column: "ComputerId");

            migrationBuilder.AddForeignKey(
                name: "FK_computer_history_computer_ComputerId",
                table: "computer_history",
                column: "ComputerId",
                principalTable: "computer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
