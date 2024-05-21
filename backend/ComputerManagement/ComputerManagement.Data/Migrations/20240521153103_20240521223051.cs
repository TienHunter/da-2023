using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20240521223051 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_computer_computer_room_ComputerRoomId",
                table: "computer");

            migrationBuilder.DropIndex(
                name: "IX_computer_ComputerRoomId",
                table: "computer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_computer_ComputerRoomId",
                table: "computer",
                column: "ComputerRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_computer_computer_room_ComputerRoomId",
                table: "computer",
                column: "ComputerRoomId",
                principalTable: "computer_room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
