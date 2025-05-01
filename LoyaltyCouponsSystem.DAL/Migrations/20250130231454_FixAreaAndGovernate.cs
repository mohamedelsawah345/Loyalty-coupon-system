using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixAreaAndGovernate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliverFromRepToCousts_Areas_AreasId",
                table: "DeliverFromRepToCousts");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliverFromRepToCousts_Governorates_GovernoratesId",
                table: "DeliverFromRepToCousts");

            migrationBuilder.DropColumn(
                name: "AreaName",
                table: "DeliverFromRepToCousts");

            migrationBuilder.DropColumn(
                name: "GovernorateName",
                table: "DeliverFromRepToCousts");

            migrationBuilder.RenameColumn(
                name: "GovernoratesId",
                table: "DeliverFromRepToCousts",
                newName: "GovernorateId");

            migrationBuilder.RenameColumn(
                name: "AreasId",
                table: "DeliverFromRepToCousts",
                newName: "AreaId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliverFromRepToCousts_GovernoratesId",
                table: "DeliverFromRepToCousts",
                newName: "IX_DeliverFromRepToCousts_GovernorateId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliverFromRepToCousts_AreasId",
                table: "DeliverFromRepToCousts",
                newName: "IX_DeliverFromRepToCousts_AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverFromRepToCousts_Areas_AreaId",
                table: "DeliverFromRepToCousts",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverFromRepToCousts_Governorates_GovernorateId",
                table: "DeliverFromRepToCousts",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliverFromRepToCousts_Areas_AreaId",
                table: "DeliverFromRepToCousts");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliverFromRepToCousts_Governorates_GovernorateId",
                table: "DeliverFromRepToCousts");

            migrationBuilder.RenameColumn(
                name: "GovernorateId",
                table: "DeliverFromRepToCousts",
                newName: "GovernoratesId");

            migrationBuilder.RenameColumn(
                name: "AreaId",
                table: "DeliverFromRepToCousts",
                newName: "AreasId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliverFromRepToCousts_GovernorateId",
                table: "DeliverFromRepToCousts",
                newName: "IX_DeliverFromRepToCousts_GovernoratesId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliverFromRepToCousts_AreaId",
                table: "DeliverFromRepToCousts",
                newName: "IX_DeliverFromRepToCousts_AreasId");

            migrationBuilder.AddColumn<string>(
                name: "AreaName",
                table: "DeliverFromRepToCousts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GovernorateName",
                table: "DeliverFromRepToCousts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverFromRepToCousts_Areas_AreasId",
                table: "DeliverFromRepToCousts",
                column: "AreasId",
                principalTable: "Areas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverFromRepToCousts_Governorates_GovernoratesId",
                table: "DeliverFromRepToCousts",
                column: "GovernoratesId",
                principalTable: "Governorates",
                principalColumn: "Id");
        }
    }
}
