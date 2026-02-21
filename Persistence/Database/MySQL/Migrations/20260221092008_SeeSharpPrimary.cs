using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Database.MySQL.Migrations
{
    /// <inheritdoc />
    public partial class SeeSharpPrimary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("01960aec-bac7-71c5-cfb0-309df6c12572"),
                columns: new[] { "Email", "UserName" },
                values: new object[] { "lmao@gmail.com", "hehe" });

            migrationBuilder.UpdateData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("019758f1-5449-87e0-d68b-e53ea6f1fb6b"),
                columns: new[] { "ExpiredDate", "StartDate" },
                values: new object[] { new DateOnly(2027, 2, 21), new DateOnly(2026, 2, 21) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("01960aec-bac7-71c5-cfb0-309df6c12572"),
                columns: new[] { "Email", "UserName" },
                values: new object[] { "kyp194490@gmail.com", "PhanKy" });

            migrationBuilder.UpdateData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("019758f1-5449-87e0-d68b-e53ea6f1fb6b"),
                columns: new[] { "ExpiredDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 6, 19), new DateOnly(2025, 6, 19) });
        }
    }
}
