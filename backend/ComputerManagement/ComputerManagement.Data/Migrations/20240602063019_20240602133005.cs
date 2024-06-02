using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20240602133005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoftwareName",
                table: "computer_software");

            migrationBuilder.AddColumn<bool>(
                name: "IsDowloadFile",
                table: "computer_software",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_computer_software_software_SoftwareId",
                table: "computer_software");

            migrationBuilder.DropIndex(
                name: "IX_computer_software_SoftwareId",
                table: "computer_software");

            migrationBuilder.DropColumn(
                name: "IsDowloadFile",
                table: "computer_software");

            migrationBuilder.AddColumn<string>(
                name: "SoftwareName",
                table: "computer_software",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
