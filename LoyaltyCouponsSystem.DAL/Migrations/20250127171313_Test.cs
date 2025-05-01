using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianCustomer_Customers_CustomerId",
                table: "TechnicianCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianCustomer_Technicians_TechnicianId",
                table: "TechnicianCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianUser_AspNetUsers_UserId",
                table: "TechnicianUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianUser_Technicians_TechnicianId",
                table: "TechnicianUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TechnicianUser",
                table: "TechnicianUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TechnicianCustomer",
                table: "TechnicianCustomer");

            migrationBuilder.RenameTable(
                name: "TechnicianUser",
                newName: "TechnicianUsers");

            migrationBuilder.RenameTable(
                name: "TechnicianCustomer",
                newName: "TechnicianCustomers");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicianUser_UserId",
                table: "TechnicianUsers",
                newName: "IX_TechnicianUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicianCustomer_CustomerId",
                table: "TechnicianCustomers",
                newName: "IX_TechnicianCustomers_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechnicianUsers",
                table: "TechnicianUsers",
                columns: new[] { "TechnicianId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechnicianCustomers",
                table: "TechnicianCustomers",
                columns: new[] { "TechnicianId", "CustomerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianCustomers_Customers_CustomerId",
                table: "TechnicianCustomers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianCustomers_Technicians_TechnicianId",
                table: "TechnicianCustomers",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "TechnicianID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianUsers_AspNetUsers_UserId",
                table: "TechnicianUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianUsers_Technicians_TechnicianId",
                table: "TechnicianUsers",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "TechnicianID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianCustomers_Customers_CustomerId",
                table: "TechnicianCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianCustomers_Technicians_TechnicianId",
                table: "TechnicianCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianUsers_AspNetUsers_UserId",
                table: "TechnicianUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianUsers_Technicians_TechnicianId",
                table: "TechnicianUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TechnicianUsers",
                table: "TechnicianUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TechnicianCustomers",
                table: "TechnicianCustomers");

            migrationBuilder.RenameTable(
                name: "TechnicianUsers",
                newName: "TechnicianUser");

            migrationBuilder.RenameTable(
                name: "TechnicianCustomers",
                newName: "TechnicianCustomer");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicianUsers_UserId",
                table: "TechnicianUser",
                newName: "IX_TechnicianUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicianCustomers_CustomerId",
                table: "TechnicianCustomer",
                newName: "IX_TechnicianCustomer_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechnicianUser",
                table: "TechnicianUser",
                columns: new[] { "TechnicianId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechnicianCustomer",
                table: "TechnicianCustomer",
                columns: new[] { "TechnicianId", "CustomerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianCustomer_Customers_CustomerId",
                table: "TechnicianCustomer",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianCustomer_Technicians_TechnicianId",
                table: "TechnicianCustomer",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "TechnicianID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianUser_AspNetUsers_UserId",
                table: "TechnicianUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianUser_Technicians_TechnicianId",
                table: "TechnicianUser",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "TechnicianID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
