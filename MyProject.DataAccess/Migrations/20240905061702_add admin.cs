using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "IsActive" },
                values: new object[] { new DateTime(2024, 9, 5, 8, 17, 2, 83, DateTimeKind.Local).AddTicks(9619), true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "IsActive" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false });
        }
    }
}
