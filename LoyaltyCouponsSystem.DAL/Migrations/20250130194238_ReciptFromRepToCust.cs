using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReciptFromRepToCust : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionForRecieptFromRepToCustID",
                table: "Technicians",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionForRecieptFromRepToCustID",
                table: "StoreKeepers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionForRecieptFromRepToCustID",
                table: "Representatives",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TransactionForRecieptFromRepToCusts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TechnitionCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReprsentitiveCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExchangePermissionNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepresentativeId = table.Column<int>(type: "int", nullable: true),
                    TechnicianId = table.Column<int>(type: "int", nullable: true),
                    StorekeeperID = table.Column<int>(type: "int", nullable: true),
                    GeneratedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GovernorateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GovernoratesId = table.Column<int>(type: "int", nullable: true),
                    AreasId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionForRecieptFromRepToCusts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TransactionForRecieptFromRepToCusts_Areas_AreasId",
                        column: x => x.AreasId,
                        principalTable: "Areas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionForRecieptFromRepToCusts_Governorates_GovernoratesId",
                        column: x => x.GovernoratesId,
                        principalTable: "Governorates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_TransactionForRecieptFromRepToCustID",
                table: "Technicians",
                column: "TransactionForRecieptFromRepToCustID");

            migrationBuilder.CreateIndex(
                name: "IX_StoreKeepers_TransactionForRecieptFromRepToCustID",
                table: "StoreKeepers",
                column: "TransactionForRecieptFromRepToCustID");

            migrationBuilder.CreateIndex(
                name: "IX_Representatives_TransactionForRecieptFromRepToCustID",
                table: "Representatives",
                column: "TransactionForRecieptFromRepToCustID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionForRecieptFromRepToCusts_AreasId",
                table: "TransactionForRecieptFromRepToCusts",
                column: "AreasId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionForRecieptFromRepToCusts_GovernoratesId",
                table: "TransactionForRecieptFromRepToCusts",
                column: "GovernoratesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Representatives_TransactionForRecieptFromRepToCusts_TransactionForRecieptFromRepToCustID",
                table: "Representatives",
                column: "TransactionForRecieptFromRepToCustID",
                principalTable: "TransactionForRecieptFromRepToCusts",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreKeepers_TransactionForRecieptFromRepToCusts_TransactionForRecieptFromRepToCustID",
                table: "StoreKeepers",
                column: "TransactionForRecieptFromRepToCustID",
                principalTable: "TransactionForRecieptFromRepToCusts",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_TransactionForRecieptFromRepToCusts_TransactionForRecieptFromRepToCustID",
                table: "Technicians",
                column: "TransactionForRecieptFromRepToCustID",
                principalTable: "TransactionForRecieptFromRepToCusts",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Representatives_TransactionForRecieptFromRepToCusts_TransactionForRecieptFromRepToCustID",
                table: "Representatives");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreKeepers_TransactionForRecieptFromRepToCusts_TransactionForRecieptFromRepToCustID",
                table: "StoreKeepers");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_TransactionForRecieptFromRepToCusts_TransactionForRecieptFromRepToCustID",
                table: "Technicians");

            migrationBuilder.DropTable(
                name: "TransactionForRecieptFromRepToCusts");

            migrationBuilder.DropIndex(
                name: "IX_Technicians_TransactionForRecieptFromRepToCustID",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_StoreKeepers_TransactionForRecieptFromRepToCustID",
                table: "StoreKeepers");

            migrationBuilder.DropIndex(
                name: "IX_Representatives_TransactionForRecieptFromRepToCustID",
                table: "Representatives");

            migrationBuilder.DropColumn(
                name: "TransactionForRecieptFromRepToCustID",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "TransactionForRecieptFromRepToCustID",
                table: "StoreKeepers");

            migrationBuilder.DropColumn(
                name: "TransactionForRecieptFromRepToCustID",
                table: "Representatives");
        }
    }
}
