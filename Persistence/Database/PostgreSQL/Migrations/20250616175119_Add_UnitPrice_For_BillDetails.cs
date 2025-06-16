using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Database.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class Add_UnitPrice_For_BillDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "BillDetails",
                type: "decimal",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("019758f1-5449-87e0-d68b-e53ea6f1fb6b"),
                columns: new[] { "ExpiredDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 6, 17), new DateOnly(2025, 6, 17) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "BillDetails");

            migrationBuilder.UpdateData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("019758f1-5449-87e0-d68b-e53ea6f1fb6b"),
                columns: new[] { "ExpiredDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 6, 16), new DateOnly(2025, 6, 16) });
        }
    }
}
