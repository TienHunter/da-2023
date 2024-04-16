using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20240416212303 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PositionRow",
                table: "computer",
                newName: "Row");

            migrationBuilder.RenameColumn(
                name: "PositionCol",
                table: "computer",
                newName: "Col");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Row",
                table: "computer",
                newName: "PositionRow");

            migrationBuilder.RenameColumn(
                name: "Col",
                table: "computer",
                newName: "PositionCol");
        }
    }
}
