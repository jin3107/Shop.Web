using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Web.Models.Migrations
{
    /// <inheritdoc />
    public partial class Update_DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Product",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "OrderItem",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Order",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }
    }
}
