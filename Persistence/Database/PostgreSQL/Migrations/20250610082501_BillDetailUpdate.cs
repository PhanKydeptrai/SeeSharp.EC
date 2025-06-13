using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Database.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class BillDetailUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("019753b7-6e9e-ebdb-7c19-1d317b5cc05f"));

            migrationBuilder.CreateTable(
                name: "BillDetails",
                columns: table => new
                {
                    BillDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    BillId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    VariantName = table.Column<string>(type: "text", nullable: false),
                    ProductVariantPrice = table.Column<decimal>(type: "decimal", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    BillDetailQuantity = table.Column<int>(type: "integer", nullable: false),
                    ColorCode = table.Column<string>(type: "text", nullable: false),
                    ProductVariantDescription = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDetails", x => x.BillDetailId);
                    table.ForeignKey(
                        name: "FK_BillDetails_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Vouchers",
                columns: new[] { "VoucherId", "Description", "ExpiredDate", "MaximumDiscountAmount", "MinimumOrderAmount", "PercentageDiscount", "StartDate", "Status", "VoucherCode", "VoucherName", "VoucherType" },
                values: new object[] { new Guid("019758f1-5449-87e0-d68b-e53ea6f1fb6b"), "Voucher dành riêng cho khách hàng mới đăng ký tài khoản", new DateOnly(2026, 6, 10), 10000m, 100000m, 0, new DateOnly(2025, 6, 10), 1, "NEWUSER01", "NEWUSER01", 0 });

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_BillId",
                table: "BillDetails",
                column: "BillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillDetails");

            migrationBuilder.DeleteData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("019758f1-5449-87e0-d68b-e53ea6f1fb6b"));

            migrationBuilder.InsertData(
                table: "Vouchers",
                columns: new[] { "VoucherId", "Description", "ExpiredDate", "MaximumDiscountAmount", "MinimumOrderAmount", "PercentageDiscount", "StartDate", "Status", "VoucherCode", "VoucherName", "VoucherType" },
                values: new object[] { new Guid("019753b7-6e9e-ebdb-7c19-1d317b5cc05f"), "Voucher dành riêng cho khách hàng mới đăng ký tài khoản", new DateOnly(2026, 6, 9), 10000m, 100000m, 0, new DateOnly(2025, 6, 9), 1, "NEWUSER01", "NEWUSER01", 0 });
        }
    }
}
