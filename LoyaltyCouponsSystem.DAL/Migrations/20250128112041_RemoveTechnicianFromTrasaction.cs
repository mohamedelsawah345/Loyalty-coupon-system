using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTechnicianFromTrasaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Technicians_TechnicianID",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "TechnicianID",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Technicians_TechnicianID",
                table: "Transactions",
                column: "TechnicianID",
                principalTable: "Technicians",
                principalColumn: "TechnicianID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Technicians_TechnicianID",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "TechnicianID",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Technicians_TechnicianID",
                table: "Transactions",
                column: "TechnicianID",
                principalTable: "Technicians",
                principalColumn: "TechnicianID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
