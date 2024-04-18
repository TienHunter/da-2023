using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20240418161447 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condition",
                table: "computer");

            migrationBuilder.AddColumn<string>(
                name: "ListErrorId",
                table: "computer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListErrorId",
                table: "computer");

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "computer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
