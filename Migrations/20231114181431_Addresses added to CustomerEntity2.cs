using ConsolShopV2.Models.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsolShopV2.Migrations
{
    /// <inheritdoc />
    public partial class AddressesaddedtoCustomerEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
    name: "Addresses",
    table: "Customers",
    nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
