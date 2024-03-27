using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.BO.Migrations
{
    /// <inheritdoc />
    public partial class _20240327214326 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Computer_ComputerRoom_ComputerRoomId",
                table: "Computer");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleBookRoom_ComputerRoom_ComputerRoomId",
                table: "ScheduleBookRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleBookRoom_Users_UserId",
                table: "ScheduleBookRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComputerRoom",
                table: "ComputerRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Computer",
                table: "Computer");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "ComputerRoom",
                newName: "ComputerRooms");

            migrationBuilder.RenameTable(
                name: "Computer",
                newName: "Computers");

            migrationBuilder.RenameIndex(
                name: "IX_Computer_ComputerRoomId",
                table: "Computers",
                newName: "IX_Computers_ComputerRoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "PK_Computers",
                table: "Computers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComputerRooms",
                table: "ComputerRooms");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Computers",
                newName: "Computer");

            migrationBuilder.RenameTable(
                name: "ComputerRooms",
                newName: "ComputerRoom");

            migrationBuilder.RenameIndex(
                name: "IX_Computers_ComputerRoomId",
                table: "Computer",
                newName: "IX_Computer_ComputerRoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Computer",
                table: "Computer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComputerRoom",
                table: "ComputerRoom",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Computer_ComputerRoom_ComputerRoomId",
                table: "Computer",
                column: "ComputerRoomId",
                principalTable: "ComputerRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleBookRoom_ComputerRoom_ComputerRoomId",
                table: "ScheduleBookRoom",
                column: "ComputerRoomId",
                principalTable: "ComputerRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleBookRoom_Users_UserId",
                table: "ScheduleBookRoom",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
