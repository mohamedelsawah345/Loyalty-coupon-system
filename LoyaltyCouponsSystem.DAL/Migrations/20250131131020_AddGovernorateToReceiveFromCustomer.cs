using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddGovernorateToReceiveFromCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "ReceiveFromCustomers");

            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "ReceiveFromCustomers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveFromCustomers_AreaId",
                table: "ReceiveFromCustomers",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveFromCustomers_GovernorateId",
                table: "ReceiveFromCustomers",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveFromCustomers_Areas_AreaId",
                table: "ReceiveFromCustomers",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveFromCustomers_Governorates_GovernorateId",
                table: "ReceiveFromCustomers",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveFromCustomers_Areas_AreaId",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveFromCustomers_Governorates_GovernorateId",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropIndex(
                name: "IX_ReceiveFromCustomers_AreaId",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropIndex(
                name: "IX_ReceiveFromCustomers_GovernorateId",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "ReceiveFromCustomers");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "ReceiveFromCustomers");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ReceiveFromCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "ReceiveFromCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
