using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Database.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewBillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_ShippingInformations_ShippingInformationId",
                table: "Bills");

            migrationBuilder.DeleteData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("01973077-7337-07e6-f542-45c86d0237f9"));

            migrationBuilder.AlterColumn<string>(
                name: "SpecificAddress",
                table: "ShippingInformations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShippingInformationId",
                table: "Bills",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Bills",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Bills",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Bills",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Bills",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecificAddress",
                table: "Bills",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ward",
                table: "Bills",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Vouchers",
                columns: new[] { "VoucherId", "Description", "ExpiredDate", "MaximumDiscountAmount", "MinimumOrderAmount", "PercentageDiscount", "StartDate", "Status", "VoucherCode", "VoucherName", "VoucherType" },
                values: new object[] { new Guid("01973b28-93a4-f06f-3ba8-31876d66966d"), "Voucher dành riêng cho khách hàng mới đăng ký tài khoản", new DateOnly(2026, 6, 4), 10000m, 100000m, 0, new DateOnly(2025, 6, 4), 1, "NEWUSER01", "NEWUSER01", 0 });

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_ShippingInformations_ShippingInformationId",
                table: "Bills",
                column: "ShippingInformationId",
                principalTable: "ShippingInformations",
                principalColumn: "ShippingInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_ShippingInformations_ShippingInformationId",
                table: "Bills");

            migrationBuilder.DeleteData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("01973b28-93a4-f06f-3ba8-31876d66966d"));

            migrationBuilder.DropColumn(
                name: "District",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "SpecificAddress",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "Ward",
                table: "Bills");

            migrationBuilder.AlterColumn<string>(
                name: "SpecificAddress",
                table: "ShippingInformations",
                type: "varchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShippingInformationId",
                table: "Bills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Vouchers",
                columns: new[] { "VoucherId", "Description", "ExpiredDate", "MaximumDiscountAmount", "MinimumOrderAmount", "PercentageDiscount", "StartDate", "Status", "VoucherCode", "VoucherName", "VoucherType" },
                values: new object[] { new Guid("01973077-7337-07e6-f542-45c86d0237f9"), "Voucher dành riêng cho khách hàng mới đăng ký tài khoản", new DateOnly(2026, 6, 2), 10000m, 100000m, 0, new DateOnly(2025, 6, 2), 1, "NEWUSER01", "NEWUSER01", 0 });

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_ShippingInformations_ShippingInformationId",
                table: "Bills",
                column: "ShippingInformationId",
                principalTable: "ShippingInformations",
                principalColumn: "ShippingInformationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
