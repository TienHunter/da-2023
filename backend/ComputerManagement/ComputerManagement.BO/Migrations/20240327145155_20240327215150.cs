using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.BO.Migrations
{
    /// <inheritdoc />
    public partial class _20240327215150 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Computers_ComputerRooms_ComputerRoomId",
                table: "Computers");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleBookRoom_ComputerRooms_ComputerRoomId",
                table: "ScheduleBookRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleBookRoom_User_UserId",
                table: "ScheduleBookRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScheduleBookRoom",
                table: "ScheduleBookRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Computers",
                table: "Computers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComputerRooms",
                table: "ComputerRooms");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "ScheduleBookRoom",
                newName: "schedule_book_room");

            migrationBuilder.RenameTable(
                name: "Computers",
                newName: "computer");

            migrationBuilder.RenameTable(
                name: "ComputerRooms",
                newName: "computer_room");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleBookRoom_UserId",
                table: "schedule_book_room",
                newName: "IX_schedule_book_room_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleBookRoom_ComputerRoomId",
                table: "schedule_book_room",
                newName: "IX_schedule_book_room_ComputerRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Computers_ComputerRoomId",
                table: "computer",
                newName: "IX_computer_ComputerRoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_schedule_book_room",
                table: "schedule_book_room",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_computer",
                table: "computer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_computer_room",
                table: "computer_room",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_computer_computer_room_ComputerRoomId",
                table: "computer",
                column: "ComputerRoomId",
                principalTable: "computer_room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_schedule_book_room_computer_room_ComputerRoomId",
                table: "schedule_book_room",
                column: "ComputerRoomId",
                principalTable: "computer_room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_schedule_book_room_user_UserId",
                table: "schedule_book_room",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_computer_computer_room_ComputerRoomId",
                table: "computer");

            migrationBuilder.DropForeignKey(
                name: "FK_schedule_book_room_computer_room_ComputerRoomId",
                table: "schedule_book_room");

            migrationBuilder.DropForeignKey(
                name: "FK_schedule_book_room_user_UserId",
                table: "schedule_book_room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_schedule_book_room",
                table: "schedule_book_room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_computer_room",
                table: "computer_room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_computer",
                table: "computer");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "schedule_book_room",
                newName: "ScheduleBookRoom");

            migrationBuilder.RenameTable(
                name: "computer_room",
                newName: "ComputerRooms");

            migrationBuilder.RenameTable(
                name: "computer",
                newName: "Computers");

            migrationBuilder.RenameIndex(
                name: "IX_schedule_book_room_UserId",
                table: "ScheduleBookRoom",
                newName: "IX_ScheduleBookRoom_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_schedule_book_room_ComputerRoomId",
                table: "ScheduleBookRoom",
                newName: "IX_ScheduleBookRoom_ComputerRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_computer_ComputerRoomId",
                table: "Computers",
                newName: "IX_Computers_ComputerRoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScheduleBookRoom",
                table: "ScheduleBookRoom",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComputerRooms",
                table: "ComputerRooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Computers",
                table: "Computers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Computers_ComputerRooms_ComputerRoomId",
                table: "Computers",
                column: "ComputerRoomId",
                principalTable: "ComputerRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleBookRoom_ComputerRooms_ComputerRoomId",
                table: "ScheduleBookRoom",
                column: "ComputerRoomId",
                principalTable: "ComputerRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleBookRoom_User_UserId",
                table: "ScheduleBookRoom",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
