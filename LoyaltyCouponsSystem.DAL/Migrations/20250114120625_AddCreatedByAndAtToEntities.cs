using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByAndAtToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributorCustomers");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Technicians",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Technicians",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Distributors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Distributors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "Distributors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DistributorCustomer",
                columns: table => new
                {
                    DistributorID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributorCustomer", x => new { x.DistributorID, x.CustomerID });
                    table.ForeignKey(
                        name: "FK_DistributorCustomer_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributorCustomer_Distributors_DistributorID",
                        column: x => x.DistributorID,
                        principalTable: "Distributors",
                        principalColumn: "DistributorID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_CustomerID",
                table: "Distributors",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_DistributorCustomer_CustomerID",
                table: "DistributorCustomer",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Distributors_Customers_CustomerID",
                table: "Distributors",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distributors_Customers_CustomerID",
                table: "Distributors");

            migrationBuilder.DropTable(
                name: "DistributorCustomer");

            migrationBuilder.DropIndex(
                name: "IX_Distributors_CustomerID",
                table: "Distributors");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Distributors");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Distributors");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Distributors");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Customers");

            migrationBuilder.CreateTable(
                name: "DistributorCustomers",
                columns: table => new
                {
                    DistributorId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributorCustomers", x => new { x.DistributorId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_DistributorCustomers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributorCustomers_Distributors_DistributorId",
                        column: x => x.DistributorId,
                        principalTable: "Distributors",
                        principalColumn: "DistributorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistributorCustomers_CustomerId",
                table: "DistributorCustomers",
                column: "CustomerId");
        }
    }
}
