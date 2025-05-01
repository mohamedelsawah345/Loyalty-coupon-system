using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MakePhoneNumberstrinng : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Distributors_PhoneNumber1",
                table: "Distributors");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber1",
                table: "Distributors",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_PhoneNumber1",
                table: "Distributors",
                column: "PhoneNumber1",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Distributors_PhoneNumber1",
                table: "Distributors");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber1",
                table: "Distributors",
                type: "int",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_PhoneNumber1",
                table: "Distributors",
                column: "PhoneNumber1",
                unique: true,
                filter: "[PhoneNumber1] IS NOT NULL");
        }
    }
}
