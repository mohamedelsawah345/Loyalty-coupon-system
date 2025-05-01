using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrinting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "RepresentativeId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "StorekeeperID",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "TechnicianId",
                table: "Coupons");

            migrationBuilder.RenameColumn(
                name: "TechnicianName",
                table: "Coupons",
                newName: "TechnicianCode");

            migrationBuilder.RenameColumn(
                name: "StoreKeeperName",
                table: "Coupons",
                newName: "StoreKeeperCode");

            migrationBuilder.RenameColumn(
                name: "RepresentativeName",
                table: "Coupons",
                newName: "RepresentativeCode");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "Coupons",
                newName: "DistributorCode");

            migrationBuilder.AddColumn<string>(
                name: "CouponeId",
                table: "Distributors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerCode",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_CouponeId",
                table: "Distributors",
                column: "CouponeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Distributors_Coupons_CouponeId",
                table: "Distributors",
                column: "CouponeId",
                principalTable: "Coupons",
                principalColumn: "CouponeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distributors_Coupons_CouponeId",
                table: "Distributors");

            migrationBuilder.DropIndex(
                name: "IX_Distributors_CouponeId",
                table: "Distributors");

            migrationBuilder.DropColumn(
                name: "CouponeId",
                table: "Distributors");

            migrationBuilder.DropColumn(
                name: "CustomerCode",
                table: "Coupons");

            migrationBuilder.RenameColumn(
                name: "TechnicianCode",
                table: "Coupons",
                newName: "TechnicianName");

            migrationBuilder.RenameColumn(
                name: "StoreKeeperCode",
                table: "Coupons",
                newName: "StoreKeeperName");

            migrationBuilder.RenameColumn(
                name: "RepresentativeCode",
                table: "Coupons",
                newName: "RepresentativeName");

            migrationBuilder.RenameColumn(
                name: "DistributorCode",
                table: "Coupons",
                newName: "CustomerName");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Coupons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepresentativeId",
                table: "Coupons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StorekeeperID",
                table: "Coupons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TechnicianId",
                table: "Coupons",
                type: "int",
                nullable: true);
        }
    }
}
