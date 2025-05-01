using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreatedByAndHistoryTransactionQRGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeWhoGenerateID",
                table: "Coupons");

            migrationBuilder.AddColumn<int>(
                name: "QRCodeTransactionGeneratedID",
                table: "Technicians",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QRCodeTransactionGeneratedID",
                table: "StoreKeepers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QRCodeTransactionGeneratedID",
                table: "Representatives",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "qRCodeTransactionGenerateds",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromSerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToSerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfCoupones = table.Column<int>(type: "int", nullable: false),
                    TypeOfCoupone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepresentativeId = table.Column<int>(type: "int", nullable: true),
                    TechnicianId = table.Column<int>(type: "int", nullable: true),
                    StorekeeperID = table.Column<int>(type: "int", nullable: true),
                    GeneratedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Areas = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qRCodeTransactionGenerateds", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_QRCodeTransactionGeneratedID",
                table: "Technicians",
                column: "QRCodeTransactionGeneratedID");

            migrationBuilder.CreateIndex(
                name: "IX_StoreKeepers_QRCodeTransactionGeneratedID",
                table: "StoreKeepers",
                column: "QRCodeTransactionGeneratedID");

            migrationBuilder.CreateIndex(
                name: "IX_Representatives_QRCodeTransactionGeneratedID",
                table: "Representatives",
                column: "QRCodeTransactionGeneratedID");

            migrationBuilder.AddForeignKey(
                name: "FK_Representatives_qRCodeTransactionGenerateds_QRCodeTransactionGeneratedID",
                table: "Representatives",
                column: "QRCodeTransactionGeneratedID",
                principalTable: "qRCodeTransactionGenerateds",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreKeepers_qRCodeTransactionGenerateds_QRCodeTransactionGeneratedID",
                table: "StoreKeepers",
                column: "QRCodeTransactionGeneratedID",
                principalTable: "qRCodeTransactionGenerateds",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_qRCodeTransactionGenerateds_QRCodeTransactionGeneratedID",
                table: "Technicians",
                column: "QRCodeTransactionGeneratedID",
                principalTable: "qRCodeTransactionGenerateds",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Representatives_qRCodeTransactionGenerateds_QRCodeTransactionGeneratedID",
                table: "Representatives");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreKeepers_qRCodeTransactionGenerateds_QRCodeTransactionGeneratedID",
                table: "StoreKeepers");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_qRCodeTransactionGenerateds_QRCodeTransactionGeneratedID",
                table: "Technicians");

            migrationBuilder.DropTable(
                name: "qRCodeTransactionGenerateds");

            migrationBuilder.DropIndex(
                name: "IX_Technicians_QRCodeTransactionGeneratedID",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_StoreKeepers_QRCodeTransactionGeneratedID",
                table: "StoreKeepers");

            migrationBuilder.DropIndex(
                name: "IX_Representatives_QRCodeTransactionGeneratedID",
                table: "Representatives");

            migrationBuilder.DropColumn(
                name: "QRCodeTransactionGeneratedID",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "QRCodeTransactionGeneratedID",
                table: "StoreKeepers");

            migrationBuilder.DropColumn(
                name: "QRCodeTransactionGeneratedID",
                table: "Representatives");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Coupons");

            migrationBuilder.AddColumn<int>(
                name: "EmployeWhoGenerateID",
                table: "Coupons",
                type: "int",
                nullable: true);
        }
    }
}
