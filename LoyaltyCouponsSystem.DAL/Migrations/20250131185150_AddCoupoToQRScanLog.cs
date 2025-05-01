using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCoupoToQRScanLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QR_ID",
                table: "QRScanLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_QRScanLogs_QR_ID",
                table: "QRScanLogs",
                column: "QR_ID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QRScanLogs_Coupons_QR_ID",
                table: "QRScanLogs",
                column: "QR_ID",
                principalTable: "Coupons",
                principalColumn: "CouponeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRScanLogs_Coupons_QR_ID",
                table: "QRScanLogs");

            migrationBuilder.DropIndex(
                name: "IX_QRScanLogs_QR_ID",
                table: "QRScanLogs");

            migrationBuilder.AlterColumn<string>(
                name: "QR_ID",
                table: "QRScanLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
