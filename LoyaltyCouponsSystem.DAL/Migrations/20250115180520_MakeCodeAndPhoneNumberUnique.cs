using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MakeCodeAndPhoneNumberUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Technicians",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Distributors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_Code",
                table: "Technicians",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_PhoneNumber1",
                table: "Technicians",
                column: "PhoneNumber1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_Code",
                table: "Distributors",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_PhoneNumber1",
                table: "Distributors",
                column: "PhoneNumber1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Code",
                table: "Customers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Technicians_Code",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Technicians_PhoneNumber1",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Distributors_Code",
                table: "Distributors");

            migrationBuilder.DropIndex(
                name: "IX_Distributors_PhoneNumber1",
                table: "Distributors");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Code",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Technicians",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Distributors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
