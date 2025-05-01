using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationsToReceiveFromCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DistributorID",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TechnicianID",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveFromCustomers_CustomerID",
                table: "ReceiveFromCustomers",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveFromCustomers_DistributorID",
                table: "ReceiveFromCustomers",
                column: "DistributorID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveFromCustomers_TechnicianID",
                table: "ReceiveFromCustomers",
                column: "TechnicianID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveFromCustomers_Customers_CustomerID",
                table: "ReceiveFromCustomers",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveFromCustomers_Distributors_DistributorID",
                table: "ReceiveFromCustomers",
                column: "DistributorID",
                principalTable: "Distributors",
                principalColumn: "DistributorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveFromCustomers_Technicians_TechnicianID",
                table: "ReceiveFromCustomers",
                column: "TechnicianID",
                principalTable: "Technicians",
                principalColumn: "TechnicianID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveFromCustomers_Customers_CustomerID",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveFromCustomers_Distributors_DistributorID",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveFromCustomers_Technicians_TechnicianID",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropIndex(
                name: "IX_ReceiveFromCustomers_CustomerID",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropIndex(
                name: "IX_ReceiveFromCustomers_DistributorID",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropIndex(
                name: "IX_ReceiveFromCustomers_TechnicianID",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "DistributorID",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "TechnicianID",
                table: "ReceiveFromCustomers");
        }
    }
}
