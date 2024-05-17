using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20240517170237 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoftwareCode",
                table: "computer_software");

            migrationBuilder.DropColumn(
                name: "SoftwareName",
                table: "computer_software");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "computer_software");

            migrationBuilder.AddColumn<string>(
                name: "InstallationFileFolder",
                table: "software",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Process",
                table: "software",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SoftwareFolder",
                table: "software",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsInstalled",
                table: "computer_software",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SoftwareId",
                table: "computer_software",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                name: "InstallationFileFolder",
                table: "software");

            migrationBuilder.DropColumn(
                name: "Process",
                table: "software");

            migrationBuilder.DropColumn(
                name: "SoftwareFolder",
                table: "software");

            migrationBuilder.DropColumn(
                name: "IsInstalled",
                table: "computer_software");

            migrationBuilder.DropColumn(
                name: "SoftwareId",
                table: "computer_software");

            migrationBuilder.AddColumn<string>(
                name: "SoftwareCode",
                table: "computer_software",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SoftwareName",
                table: "computer_software",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "computer_software",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
