using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Database.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Feedbacks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("019758f1-5449-87e0-d68b-e53ea6f1fb6b"),
                columns: new[] { "ExpiredDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 6, 19), new DateOnly(2025, 6, 19) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Feedbacks");

            migrationBuilder.UpdateData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("019758f1-5449-87e0-d68b-e53ea6f1fb6b"),
                columns: new[] { "ExpiredDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 6, 17), new DateOnly(2025, 6, 17) });
        }
    }
}
