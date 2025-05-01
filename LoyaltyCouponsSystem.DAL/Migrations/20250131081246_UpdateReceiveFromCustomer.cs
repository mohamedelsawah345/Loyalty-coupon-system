using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReceiveFromCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "DistributorId",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "TechnicianId",
                table: "ReceiveFromCustomers");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ReceiveFromCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerCode",
                table: "ReceiveFromCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DistributorCode",
                table: "ReceiveFromCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "ReceiveFromCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TechnicianCode",
                table: "ReceiveFromCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "CustomerCode",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "DistributorCode",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "TechnicianCode",
                table: "ReceiveFromCustomers");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DistributorId",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TechnicianId",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
