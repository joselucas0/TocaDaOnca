using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TocaDaOnca.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sales_products_Products_product_id",
                table: "sales_products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                table: "products",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_sales_products_products_product_id",
                table: "sales_products",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sales_products_products_product_id",
                table: "sales_products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_sales_products_Products_product_id",
                table: "sales_products",
                column: "product_id",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
