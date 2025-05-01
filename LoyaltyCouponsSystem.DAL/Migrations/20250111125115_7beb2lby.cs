using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class _7beb2lby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Coupons_AreaId",
                table: "Coupons",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_GovernorateId",
                table: "Coupons",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Areas_AreaId",
                table: "Coupons",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Governorates_GovernorateId",
                table: "Coupons",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Areas_AreaId",
                table: "Coupons");

            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Governorates_GovernorateId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_AreaId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_GovernorateId",
                table: "Coupons");
        }
    }
}
