using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addadminuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DeletedAt", "Email", "FirstName", "IsActive", "IsDeleted", "LastName", "ModifiedAt", "Password", "RoleId", "Username" },
                values: new object[] { 1, null, "mika123@gmail.com", "Mika", false, false, "Mikic", null, "mika123", 2, "Mika" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
