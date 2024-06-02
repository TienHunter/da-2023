using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20240601165734 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "computer_history",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "computer_history",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "config_option",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OptionValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false),
                    IsAgent = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_config_option", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_config_option_OptionName",
                table: "config_option",
                column: "OptionName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "config_option");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "computer_history");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "computer_history");

            migrationBuilder.CreateTable(
                name: "refresh_token",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_token", x => x.Id);
                });
        }
    }
}
