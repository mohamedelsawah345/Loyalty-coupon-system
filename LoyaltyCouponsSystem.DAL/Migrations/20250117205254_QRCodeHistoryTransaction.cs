using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class QRCodeHistoryTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_qRCodeTransactionGenerateds_AreaId",
                table: "qRCodeTransactionGenerateds",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_qRCodeTransactionGenerateds_GovernorateID",
                table: "qRCodeTransactionGenerateds",
                column: "GovernorateID");

            migrationBuilder.AddForeignKey(
                name: "FK_qRCodeTransactionGenerateds_Areas_AreaId",
                table: "qRCodeTransactionGenerateds",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_qRCodeTransactionGenerateds_Governorates_GovernorateID",
                table: "qRCodeTransactionGenerateds",
                column: "GovernorateID",
                principalTable: "Governorates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_qRCodeTransactionGenerateds_Areas_AreaId",
                table: "qRCodeTransactionGenerateds");

            migrationBuilder.DropForeignKey(
                name: "FK_qRCodeTransactionGenerateds_Governorates_GovernorateID",
                table: "qRCodeTransactionGenerateds");

            migrationBuilder.DropIndex(
                name: "IX_qRCodeTransactionGenerateds_AreaId",
                table: "qRCodeTransactionGenerateds");

            migrationBuilder.DropIndex(
                name: "IX_qRCodeTransactionGenerateds_GovernorateID",
                table: "qRCodeTransactionGenerateds");
        }
    }
}
