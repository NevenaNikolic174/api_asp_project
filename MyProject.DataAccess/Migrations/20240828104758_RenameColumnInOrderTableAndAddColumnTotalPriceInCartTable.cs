using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnInOrderTableAndAddColumnTotalPriceInCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Orders",
                newName: "Amount");

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Carts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Orders",
                newName: "TotalPrice");
        }
    }
}
