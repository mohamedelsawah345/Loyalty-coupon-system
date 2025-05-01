using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletePropertyToDistributorCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributorCustomer_Customers_CustomerID",
                table: "DistributorCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributorCustomer_Distributors_DistributorID",
                table: "DistributorCustomer");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributorCustomer_Customers_CustomerID",
                table: "DistributorCustomer",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DistributorCustomer_Distributors_DistributorID",
                table: "DistributorCustomer",
                column: "DistributorID",
                principalTable: "Distributors",
                principalColumn: "DistributorID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributorCustomer_Customers_CustomerID",
                table: "DistributorCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributorCustomer_Distributors_DistributorID",
                table: "DistributorCustomer");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributorCustomer_Customers_CustomerID",
                table: "DistributorCustomer",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DistributorCustomer_Distributors_DistributorID",
                table: "DistributorCustomer",
                column: "DistributorID",
                principalTable: "Distributors",
                principalColumn: "DistributorID");
        }
    }
}
