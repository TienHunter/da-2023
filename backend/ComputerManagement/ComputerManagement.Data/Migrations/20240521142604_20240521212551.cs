using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20240521212551 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_computer_software_software_SoftwareId",
                table: "computer_software");

            migrationBuilder.DropIndex(
                name: "IX_computer_software_SoftwareId",
                table: "computer_software");

            migrationBuilder.DropColumn(
                name: "State",
                table: "computer_room");

            migrationBuilder.AddColumn<string>(
                name: "SoftwareName",
                table: "computer_software",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoftwareName",
                table: "computer_software");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "computer_room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_computer_software_SoftwareId",
                table: "computer_software",
                column: "SoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_computer_software_software_SoftwareId",
                table: "computer_software",
                column: "SoftwareId",
                principalTable: "software",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
