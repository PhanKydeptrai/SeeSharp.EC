using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Database.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("01973b28-93a4-f06f-3ba8-31876d66966d"));

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Bills");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Bills",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Vouchers",
                columns: new[] { "VoucherId", "Description", "ExpiredDate", "MaximumDiscountAmount", "MinimumOrderAmount", "PercentageDiscount", "StartDate", "Status", "VoucherCode", "VoucherName", "VoucherType" },
                values: new object[] { new Guid("019753b7-6e9e-ebdb-7c19-1d317b5cc05f"), "Voucher dành riêng cho khách hàng mới đăng ký tài khoản", new DateOnly(2026, 6, 9), 10000m, 100000m, 0, new DateOnly(2025, 6, 9), 1, "NEWUSER01", "NEWUSER01", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("019753b7-6e9e-ebdb-7c19-1d317b5cc05f"));

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Bills");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Bills",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Vouchers",
                columns: new[] { "VoucherId", "Description", "ExpiredDate", "MaximumDiscountAmount", "MinimumOrderAmount", "PercentageDiscount", "StartDate", "Status", "VoucherCode", "VoucherName", "VoucherType" },
                values: new object[] { new Guid("01973b28-93a4-f06f-3ba8-31876d66966d"), "Voucher dành riêng cho khách hàng mới đăng ký tài khoản", new DateOnly(2026, 6, 4), 10000m, 100000m, 0, new DateOnly(2025, 6, 4), 1, "NEWUSER01", "NEWUSER01", 0 });
        }
    }
}
