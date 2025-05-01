using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTecnicianData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactDetails",
                table: "Technicians",
                newName: "NickName");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Technicians",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Governate",
                table: "Technicians",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NationalID",
                table: "Technicians",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber1",
                table: "Technicians",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber2",
                table: "Technicians",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber3",
                table: "Technicians",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "Governate",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "NationalID",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "PhoneNumber1",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "PhoneNumber2",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "PhoneNumber3",
                table: "Technicians");

            migrationBuilder.RenameColumn(
                name: "NickName",
                table: "Technicians",
                newName: "ContactDetails");
        }
    }
}
