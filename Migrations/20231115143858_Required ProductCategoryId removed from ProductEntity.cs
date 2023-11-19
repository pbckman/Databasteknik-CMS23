using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsolShopV2.Migrations
{
    /// <inheritdoc />
    public partial class RequiredProductCategoryIdremovedfromProductEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
    name: "ProductCategoryId",
    table: "Products",
    nullable: false,
    oldClrType: typeof(int),
    oldType: "int",
    oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
    name: "ProductCategoryId",
    table: "Products",
    type: "int",
    nullable: true,
    oldClrType: typeof(int));

        }
    }
}
