using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class editonTableDeliverToCust : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliverFromRepToCousts_Customers_CustomerID",
                table: "DeliverFromRepToCousts");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliverFromRepToCousts_Technicians_TechnicianID",
                table: "DeliverFromRepToCousts");

            migrationBuilder.AlterColumn<int>(
                name: "TechnicianID",
                table: "DeliverFromRepToCousts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerID",
                table: "DeliverFromRepToCousts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverFromRepToCousts_Customers_CustomerID",
                table: "DeliverFromRepToCousts",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverFromRepToCousts_Technicians_TechnicianID",
                table: "DeliverFromRepToCousts",
                column: "TechnicianID",
                principalTable: "Technicians",
                principalColumn: "TechnicianID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliverFromRepToCousts_Customers_CustomerID",
                table: "DeliverFromRepToCousts");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliverFromRepToCousts_Technicians_TechnicianID",
                table: "DeliverFromRepToCousts");

            migrationBuilder.AlterColumn<int>(
                name: "TechnicianID",
                table: "DeliverFromRepToCousts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerID",
                table: "DeliverFromRepToCousts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverFromRepToCousts_Customers_CustomerID",
                table: "DeliverFromRepToCousts",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverFromRepToCousts_Technicians_TechnicianID",
                table: "DeliverFromRepToCousts",
                column: "TechnicianID",
                principalTable: "Technicians",
                principalColumn: "TechnicianID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
