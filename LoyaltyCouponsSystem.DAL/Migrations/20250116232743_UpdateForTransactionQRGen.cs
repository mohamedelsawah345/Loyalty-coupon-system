using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForTransactionQRGen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Areas",
                table: "qRCodeTransactionGenerateds");

            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "qRCodeTransactionGenerateds");

            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "qRCodeTransactionGenerateds",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateID",
                table: "qRCodeTransactionGenerateds",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "qRCodeTransactionGenerateds");

            migrationBuilder.DropColumn(
                name: "GovernorateID",
                table: "qRCodeTransactionGenerateds");

            migrationBuilder.AddColumn<string>(
                name: "Areas",
                table: "qRCodeTransactionGenerateds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "qRCodeTransactionGenerateds",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
