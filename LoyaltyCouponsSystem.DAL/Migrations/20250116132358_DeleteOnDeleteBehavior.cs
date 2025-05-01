using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeleteOnDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Representatives",
                newName: "PhoneNumber");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Technicians",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Representatives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Governate",
                table: "Representatives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "Representatives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OptionalPhoneNumber",
                table: "Representatives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TechnicianId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TechnicianId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TechnicianId",
                table: "Customers",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TechnicianId",
                table: "AspNetUsers",
                column: "TechnicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Technicians_TechnicianId",
                table: "AspNetUsers",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "TechnicianID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Technicians_TechnicianId",
                table: "Customers",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "TechnicianID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Technicians_TechnicianId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Technicians_TechnicianId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TechnicianId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TechnicianId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Representatives");

            migrationBuilder.DropColumn(
                name: "Governate",
                table: "Representatives");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "Representatives");

            migrationBuilder.DropColumn(
                name: "OptionalPhoneNumber",
                table: "Representatives");

            migrationBuilder.DropColumn(
                name: "TechnicianId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TechnicianId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Representatives",
                newName: "Number");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Technicians",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
