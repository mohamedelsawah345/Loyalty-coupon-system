using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ForUpdateAfterSamirAndBasunii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliverFromRepToCousts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepresintitiveCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GovernorateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GovernoratesId = table.Column<int>(type: "int", nullable: true),
                    AreasId = table.Column<int>(type: "int", nullable: true),
                    AreaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    TechnicianID = table.Column<int>(type: "int", nullable: false),
                    ExchangePermission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostomerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TechnitionCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliverFromRepToCousts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliverFromRepToCousts_Areas_AreasId",
                        column: x => x.AreasId,
                        principalTable: "Areas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeliverFromRepToCousts_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliverFromRepToCousts_Governorates_GovernoratesId",
                        column: x => x.GovernoratesId,
                        principalTable: "Governorates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeliverFromRepToCousts_Technicians_TechnicianID",
                        column: x => x.TechnicianID,
                        principalTable: "Technicians",
                        principalColumn: "TechnicianID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliverFromRepToCousts_AreasId",
                table: "DeliverFromRepToCousts",
                column: "AreasId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliverFromRepToCousts_CustomerID",
                table: "DeliverFromRepToCousts",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_DeliverFromRepToCousts_GovernoratesId",
                table: "DeliverFromRepToCousts",
                column: "GovernoratesId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliverFromRepToCousts_TechnicianID",
                table: "DeliverFromRepToCousts",
                column: "TechnicianID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliverFromRepToCousts");
        }
    }
}
