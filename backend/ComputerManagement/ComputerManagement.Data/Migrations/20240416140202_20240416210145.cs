using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20240416210145 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PositionY",
                table: "computer",
                newName: "PositionRow");

            migrationBuilder.RenameColumn(
                name: "PositionX",
                table: "computer",
                newName: "PositionCol");

            migrationBuilder.AddColumn<int>(
                name: "Col",
                table: "computer_room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Row",
                table: "computer_room",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Col",
                table: "computer_room");

            migrationBuilder.DropColumn(
                name: "Row",
                table: "computer_room");

            migrationBuilder.RenameColumn(
                name: "PositionRow",
                table: "computer",
                newName: "PositionY");

            migrationBuilder.RenameColumn(
                name: "PositionCol",
                table: "computer",
                newName: "PositionX");
        }
    }
}
