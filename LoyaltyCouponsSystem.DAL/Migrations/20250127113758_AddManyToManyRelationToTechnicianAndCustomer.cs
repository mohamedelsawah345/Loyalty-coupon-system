using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyRelationToTechnicianAndCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Technicians_TechnicianId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Technicians_TechnicianId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TechnicianId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TechnicianId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TechnicianId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TechnicianId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "TechnicianCustomer",
                columns: table => new
                {
                    TechnicianId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianCustomer", x => new { x.TechnicianId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_TechnicianCustomer_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicianCustomer_Technicians_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Technicians",
                        principalColumn: "TechnicianID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnicianUser",
                columns: table => new
                {
                    TechnicianId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianUser", x => new { x.TechnicianId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TechnicianUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicianUser_Technicians_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Technicians",
                        principalColumn: "TechnicianID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianCustomer_CustomerId",
                table: "TechnicianCustomer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianUser_UserId",
                table: "TechnicianUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TechnicianCustomer");

            migrationBuilder.DropTable(
                name: "TechnicianUser");

            migrationBuilder.AddColumn<int>(
                name: "TechnicianId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TechnicianId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TechnicianId",
                table: "Customers",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TechnicianId",
                table: "AspNetUsers",
                column: "TechnicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Technicians_TechnicianId",
                table: "AspNetUsers",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "TechnicianID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Technicians_TechnicianId",
                table: "Customers",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "TechnicianID");
        }
    }
}
