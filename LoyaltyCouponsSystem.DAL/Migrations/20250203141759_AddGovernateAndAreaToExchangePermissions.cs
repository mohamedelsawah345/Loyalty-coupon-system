using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddGovernateAndAreaToExchangePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Governate",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "AreaName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GovernorateName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "GovernorateName",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Governate",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
