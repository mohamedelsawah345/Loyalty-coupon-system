using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MakePhoneNumberNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Technicians_PhoneNumber1",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Distributors_PhoneNumber1",
                table: "Distributors");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber1",
                table: "Technicians",
                type: "int",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber1",
                table: "Distributors",
                type: "int",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_PhoneNumber1",
                table: "Technicians",
                column: "PhoneNumber1",
                unique: true,
                filter: "[PhoneNumber1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_PhoneNumber1",
                table: "Distributors",
                column: "PhoneNumber1",
                unique: true,
                filter: "[PhoneNumber1] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Technicians_PhoneNumber1",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Distributors_PhoneNumber1",
                table: "Distributors");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber1",
                table: "Technicians",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber1",
                table: "Distributors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_PhoneNumber1",
                table: "Technicians",
                column: "PhoneNumber1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_PhoneNumber1",
                table: "Distributors",
                column: "PhoneNumber1",
                unique: true);
        }
    }
}
