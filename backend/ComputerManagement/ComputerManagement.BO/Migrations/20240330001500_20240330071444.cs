using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.BO.Migrations
{
    /// <inheritdoc />
    public partial class _20240330071444 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "user");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "user");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "user");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "user");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "schedule_book_room");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "schedule_book_room");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "schedule_book_room");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "schedule_book_room");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "computer_room");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "computer_room");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "computer_room");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "computer_room");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "computer");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "computer");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "computer");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "computer");

            migrationBuilder.RenameColumn(
                name: "Usermane",
                table: "user",
                newName: "Username");

            migrationBuilder.CreateTable(
                name: "BaseModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseModel", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_computer_BaseModel_Id",
                table: "computer",
                column: "Id",
                principalTable: "BaseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_computer_room_BaseModel_Id",
                table: "computer_room",
                column: "Id",
                principalTable: "BaseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_schedule_book_room_BaseModel_Id",
                table: "schedule_book_room",
                column: "Id",
                principalTable: "BaseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_BaseModel_Id",
                table: "user",
                column: "Id",
                principalTable: "BaseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_computer_BaseModel_Id",
                table: "computer");

            migrationBuilder.DropForeignKey(
                name: "FK_computer_room_BaseModel_Id",
                table: "computer_room");

            migrationBuilder.DropForeignKey(
                name: "FK_schedule_book_room_BaseModel_Id",
                table: "schedule_book_room");

            migrationBuilder.DropForeignKey(
                name: "FK_user_BaseModel_Id",
                table: "user");

            migrationBuilder.DropTable(
                name: "BaseModel");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "user",
                newName: "Usermane");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "user",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "user",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "user",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "user",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "schedule_book_room",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "schedule_book_room",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "schedule_book_room",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "schedule_book_room",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "computer_room",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "computer_room",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "computer_room",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "computer_room",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "computer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "computer",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "computer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "computer",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
