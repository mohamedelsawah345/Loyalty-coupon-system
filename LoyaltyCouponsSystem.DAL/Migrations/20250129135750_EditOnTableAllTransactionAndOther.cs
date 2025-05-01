using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditOnTableAllTransactionAndOther : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FlagToPrint",
                table: "qRCodeTransactionGenerateds",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CouponeId",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Coupons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "FlagToPrint",
                table: "Coupons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RepresentativeName",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreKeeperName",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TechnicianName",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CouponeId",
                table: "Customers",
                column: "CouponeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Coupons_CouponeId",
                table: "Customers",
                column: "CouponeId",
                principalTable: "Coupons",
                principalColumn: "CouponeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Coupons_CouponeId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CouponeId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FlagToPrint",
                table: "qRCodeTransactionGenerateds");

            migrationBuilder.DropColumn(
                name: "CouponeId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "FlagToPrint",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "RepresentativeName",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "StoreKeeperName",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "TechnicianName",
                table: "Coupons");
        }
    }
}
