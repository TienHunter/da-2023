using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20240331110209 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user",
                keyColumn: "Id",
                keyValue: new Guid("384c75b9-088a-4ca9-bb92-0f2c754f8e2f"));

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "Fullname", "Password", "RoleID", "RoleIDText", "State", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("ff34c28a-9176-4c48-b0b2-90b4e844ef5d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "admin@gmail.com", "admin", "1234", 1, null, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user",
                keyColumn: "Id",
                keyValue: new Guid("ff34c28a-9176-4c48-b0b2-90b4e844ef5d"));

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "Fullname", "Password", "RoleID", "RoleIDText", "State", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("384c75b9-088a-4ca9-bb92-0f2c754f8e2f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "admin@gmail.com", "admin", "1234", 1, null, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "admin" });
        }
    }
}
