using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddGovernateAndAreaToExchangePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AreaId",
                table: "Transactions",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_GovernorateId",
                table: "Transactions",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Areas_AreaId",
                table: "Transactions",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Governorates_GovernorateId",
                table: "Transactions",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Areas_AreaId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Governorates_GovernorateId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AreaId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_GovernorateId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Transactions");
        }
    }
}
