using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyaltyCouponsSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedForSequenceNumWithType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxSerialNumber",
                table: "GlobalCounters",
                newName: "MaxSerialNumber6");

            migrationBuilder.AddColumn<long>(
                name: "MaxSerialNumber1",
                table: "GlobalCounters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MaxSerialNumber2",
                table: "GlobalCounters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MaxSerialNumber3",
                table: "GlobalCounters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MaxSerialNumber4",
                table: "GlobalCounters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MaxSerialNumber5",
                table: "GlobalCounters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxSerialNumber1",
                table: "GlobalCounters");

            migrationBuilder.DropColumn(
                name: "MaxSerialNumber2",
                table: "GlobalCounters");

            migrationBuilder.DropColumn(
                name: "MaxSerialNumber3",
                table: "GlobalCounters");

            migrationBuilder.DropColumn(
                name: "MaxSerialNumber4",
                table: "GlobalCounters");

            migrationBuilder.DropColumn(
                name: "MaxSerialNumber5",
                table: "GlobalCounters");

            migrationBuilder.RenameColumn(
                name: "MaxSerialNumber6",
                table: "GlobalCounters",
                newName: "MaxSerialNumber");
        }
    }
}
