using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableToReceiveFromCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "TechnicianID",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DistributorID",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerID",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveFromCustomers_Customers_CustomerID",
                table: "ReceiveFromCustomers",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveFromCustomers_Distributors_DistributorID",
                table: "ReceiveFromCustomers",
                column: "DistributorID",
                principalTable: "Distributors",
                principalColumn: "DistributorID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveFromCustomers_Technicians_TechnicianID",
                table: "ReceiveFromCustomers",
                column: "TechnicianID",
                principalTable: "Technicians",
                principalColumn: "TechnicianID");
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

            migrationBuilder.AlterColumn<int>(
                name: "TechnicianID",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DistributorID",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerID",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
