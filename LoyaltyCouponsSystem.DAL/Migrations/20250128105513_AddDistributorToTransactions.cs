using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDistributorToTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistributorID",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DistributorID",
                table: "Transactions",
                column: "DistributorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Distributors_DistributorID",
                table: "Transactions",
                column: "DistributorID",
                principalTable: "Distributors",
                principalColumn: "DistributorID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Distributors_DistributorID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_DistributorID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DistributorID",
                table: "Transactions");
        }
    }
}
