using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Database.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(64)");

            migrationBuilder.UpdateData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("019758f1-5449-87e0-d68b-e53ea6f1fb6b"),
                columns: new[] { "ExpiredDate", "StartDate" },
                values: new object[] { new DateOnly(2027, 3, 11), new DateOnly(2026, 3, 11) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "varchar(64)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.UpdateData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("019758f1-5449-87e0-d68b-e53ea6f1fb6b"),
                columns: new[] { "ExpiredDate", "StartDate" },
                values: new object[] { new DateOnly(2027, 3, 10), new DateOnly(2026, 3, 10) });
        }
    }
}
