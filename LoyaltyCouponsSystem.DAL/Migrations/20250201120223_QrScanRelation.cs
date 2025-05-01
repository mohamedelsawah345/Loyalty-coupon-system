using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class QrScanRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "CouponId",
                table: "QRScanLogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QRScanLogId",
                table: "Coupons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QRScanLogs_CouponId",
                table: "QRScanLogs",
                column: "CouponId",
                unique: true,
                filter: "[CouponId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_QRScanLogs_Coupons_CouponId",
                table: "QRScanLogs",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "CouponeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRScanLogs_Coupons_CouponId",
                table: "QRScanLogs");

            migrationBuilder.DropIndex(
                name: "IX_QRScanLogs_CouponId",
                table: "QRScanLogs");

            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "QRScanLogs");

            migrationBuilder.DropColumn(
                name: "QRScanLogId",
                table: "Coupons");

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
    }
}
