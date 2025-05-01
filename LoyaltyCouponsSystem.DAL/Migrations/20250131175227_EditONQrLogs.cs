using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditONQrLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfScans",
                table: "QRScanLogs");

            migrationBuilder.RenameColumn(
                name: "UserIP",
                table: "QRScanLogs",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "UserAgent",
                table: "QRScanLogs",
                newName: "ScanedBy");

            migrationBuilder.AddColumn<string>(
                name: "ReceiptNumber",
                table: "QRScanLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptNumber",
                table: "QRScanLogs");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "QRScanLogs",
                newName: "UserIP");

            migrationBuilder.RenameColumn(
                name: "ScanedBy",
                table: "QRScanLogs",
                newName: "UserAgent");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfScans",
                table: "QRScanLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
