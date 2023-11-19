using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsolShopV2.Migrations
{
    /// <inheritdoc />
    public partial class NavrelationaddedbetweenprodCatEntandprodEnt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCategories_NameCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_NameCategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NameCategoryId",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCategories_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCategories_ProductCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "NameCategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_NameCategoryId",
                table: "Products",
                column: "NameCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCategories_NameCategoryId",
                table: "Products",
                column: "NameCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
