using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Database.MySQL.Migrations
{
    /// <inheritdoc />
    public partial class NextSharpPrimary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    CategoryName = table.Column<string>(type: "varchar(50)", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(200)", nullable: true),
                    CategoryStatus = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<sbyte>(type: "tinyint(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "JSON", nullable: false),
                    OccurredOnUtc = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    Error = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    UserName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Email = table.Column<string>(type: "varchar(200)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(64)", nullable: true),
                    UserStatus = table.Column<int>(type: "int", nullable: false),
                    IsVerify = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    ImageUrl = table.Column<string>(type: "varchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    VoucherId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    VoucherName = table.Column<string>(type: "varchar(20)", nullable: false),
                    VoucherCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    PercentageDiscount = table.Column<int>(type: "int", nullable: false),
                    MaximumDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.VoucherId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    ProductName = table.Column<string>(type: "varchar(50)", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(255)", nullable: true),
                    Description = table.Column<string>(type: "varchar(255)", nullable: true),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductStatus = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    CustomerStatus = table.Column<int>(type: "int", nullable: false),
                    CustomerType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    EmployeeStatus = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserAuthenticationTokens",
                columns: table => new
                {
                    UserAuthenticationTokenId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Value = table.Column<string>(type: "varchar(500)", nullable: false),
                    TokenType = table.Column<int>(type: "int", nullable: false),
                    ExpiredTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    IsBlackList = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthenticationTokens", x => x.UserAuthenticationTokenId);
                    table.ForeignKey(
                        name: "FK_UserAuthenticationTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VerificationTokens",
                columns: table => new
                {
                    VerificationTokenId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Temporary = table.Column<string>(type: "varchar(64)", nullable: true),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    CreatedDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationTokens", x => x.VerificationTokenId);
                    table.ForeignKey(
                        name: "FK_VerificationTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomerVouchers",
                columns: table => new
                {
                    CustomerVoucherId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    VoucherId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerVouchers", x => x.CustomerVoucherId);
                    table.ForeignKey(
                        name: "FK_CustomerVouchers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerVouchers_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "VoucherId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    OrderTransactionId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ShippingInformations",
                columns: table => new
                {
                    ShippingInformationId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    FullName = table.Column<string>(type: "varchar(50)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(10)", nullable: false),
                    IsDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SpecificAddress = table.Column<string>(type: "varchar(50)", nullable: false),
                    Province = table.Column<string>(type: "varchar(50)", nullable: false),
                    District = table.Column<string>(type: "varchar(50)", nullable: false),
                    Ward = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingInformations", x => x.ShippingInformationId);
                    table.ForeignKey(
                        name: "FK_ShippingInformations_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WishItems",
                columns: table => new
                {
                    WishItemId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishItems", x => x.WishItemId);
                    table.ForeignKey(
                        name: "FK_WishItems_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Substance = table.Column<string>(type: "varchar(255)", nullable: true),
                    RatingScore = table.Column<float>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(255)", nullable: true),
                    OrderId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    OrderId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BillId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    OrderId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    CreatedDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    ShippingInformationId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_Bills_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bills_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bills_ShippingInformations_ShippingInformationId",
                        column: x => x.ShippingInformationId,
                        principalTable: "ShippingInformations",
                        principalColumn: "ShippingInformationId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderTransactions",
                columns: table => new
                {
                    OrderTransactionId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    PayerName = table.Column<string>(type: "varchar(50)", nullable: true),
                    PayerEmail = table.Column<string>(type: "varchar(200)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    IsVoucherUsed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VoucherId = table.Column<Guid>(type: "char(36)", nullable: true, defaultValueSql: "(UUID())"),
                    OrderId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    BillId = table.Column<Guid>(type: "char(36)", nullable: true, defaultValueSql: "(UUID())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTransactions", x => x.OrderTransactionId);
                    table.ForeignKey(
                        name: "FK_OrderTransactions_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "BillId");
                    table.ForeignKey(
                        name: "FK_OrderTransactions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderTransactions_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "VoucherId");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "CategoryStatus", "ImageUrl", "IsDefault" },
                values: new object[] { new Guid("019546cc-2909-1710-9a1b-36df36d9a7ae"), "General", 0, "", (sbyte)1 });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_CustomerId",
                table: "Bills",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_OrderId",
                table: "Bills",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_ShippingInformationId",
                table: "Bills",
                column: "ShippingInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName",
                table: "Categories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerVouchers_CustomerId",
                table: "CustomerVouchers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerVouchers_VoucherId",
                table: "CustomerVouchers",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CustomerId",
                table: "Feedbacks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_OrderId",
                table: "Feedbacks",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTransactions_BillId",
                table: "OrderTransactions",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTransactions_OrderId",
                table: "OrderTransactions",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderTransactions_VoucherId",
                table: "OrderTransactions",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingInformations_CustomerId",
                table: "ShippingInformations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthenticationTokens_UserId",
                table: "UserAuthenticationTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationTokens_UserId",
                table: "VerificationTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WishItems_CustomerId",
                table: "WishItems",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_WishItems_ProductId",
                table: "WishItems",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerVouchers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "OrderTransactions");

            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropTable(
                name: "UserAuthenticationTokens");

            migrationBuilder.DropTable(
                name: "VerificationTokens");

            migrationBuilder.DropTable(
                name: "WishItems");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ShippingInformations");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
