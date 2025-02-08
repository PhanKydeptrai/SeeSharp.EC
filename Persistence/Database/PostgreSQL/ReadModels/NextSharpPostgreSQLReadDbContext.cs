using Microsoft.EntityFrameworkCore;

namespace Persistence.Database.PostgreSQL.ReadModels;

public partial class NextSharpPostgreSQLReadDbContext : DbContext
{
    public NextSharpPostgreSQLReadDbContext()
    {
    }

    public NextSharpPostgreSQLReadDbContext(DbContextOptions<NextSharpPostgreSQLReadDbContext> options)
        : base(options)
    {
    }

    public DbSet<BillReadModel> Bills { get; set; }

    public DbSet<CategoryReadModel> Categories { get; set; }

    public DbSet<CustomerReadModel> Customers { get; set; }

    public DbSet<CustomerVoucherReadModel> CustomerVouchers { get; set; }

    public DbSet<EmployeeReadModel> Employees { get; set; }

    public DbSet<FeedbackReadModel> Feedbacks { get; set; }

    public DbSet<OrderReadModel> Orders { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }

    public DbSet<OrderTransactionReadModel> OrderTransactions { get; set; }

    public DbSet<ProductReadModel> Products { get; set; }

    public DbSet<ShippingInformationReadModel> ShippingInformations { get; set; }

    public DbSet<UserReadModel> Users { get; set; }

    public DbSet<UserAuthenticationToken> UserAuthenticationTokens { get; set; }

    public DbSet<VerifyVerificationTokenReadModel> VerifyVerificationTokens { get; set; }

    public DbSet<VoucherReadModel> Vouchers { get; set; }

    public DbSet<WishItemReadModel> WishItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:ReadOnlyDatabase");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BillReadModel>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_Bills_CustomerId");

            entity.HasIndex(e => e.OrderId, "IX_Bills_OrderId").IsUnique();

            entity.HasIndex(e => e.ShippingInformationId, "IX_Bills_ShippingInformationId");

            entity.Property(e => e.BillId).HasMaxLength(26);
            entity.Property(e => e.CreatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CustomerId).HasMaxLength(26);
            entity.Property(e => e.OrderId).HasMaxLength(26);
            entity.Property(e => e.PaymentMethod).HasMaxLength(20);
            entity.Property(e => e.ShippingInformationId).HasMaxLength(26);

            entity.HasOne(d => d.Customer).WithMany(p => p.Bills).HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.Order).WithOne(p => p.Bill).HasForeignKey<BillReadModel>(d => d.OrderId);

            entity.HasOne(d => d.ShippingInformation).WithMany(p => p.Bills).HasForeignKey(d => d.ShippingInformationId);
        });

        modelBuilder.Entity<CategoryReadModel>(entity =>
        {
            entity.Property(e => e.CategoryId).HasMaxLength(26);
            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.CategoryStatus).HasMaxLength(20);
            entity.Property(e => e.ImageUrl).HasMaxLength(200);
        });

        modelBuilder.Entity<CustomerReadModel>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Customers_UserId").IsUnique();

            entity.Property(e => e.CustomerId).HasMaxLength(26);
            entity.Property(e => e.CustomerStatus).HasMaxLength(20);
            entity.Property(e => e.CustomerType).HasMaxLength(20);
            entity.Property(e => e.UserId).HasMaxLength(26);

            entity.HasOne(d => d.User).WithOne(p => p.Customer).HasForeignKey<CustomerReadModel>(d => d.UserId);
        });

        modelBuilder.Entity<CustomerVoucherReadModel>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_CustomerVouchers_CustomerId");

            entity.HasIndex(e => e.VoucherId, "IX_CustomerVouchers_VoucherId");

            entity.Property(e => e.CustomerVoucherId).HasMaxLength(26);
            entity.Property(e => e.CustomerId).HasMaxLength(26);
            entity.Property(e => e.VoucherId).HasMaxLength(26);

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerVouchers).HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.Voucher).WithMany(p => p.CustomerVouchers).HasForeignKey(d => d.VoucherId);
        });

        modelBuilder.Entity<EmployeeReadModel>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Employees_UserId").IsUnique();

            entity.Property(e => e.EmployeeId).HasMaxLength(26);
            entity.Property(e => e.EmployeeStatus).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.UserId).HasMaxLength(26);

            entity.HasOne(d => d.User).WithOne(p => p.Employee).HasForeignKey<EmployeeReadModel>(d => d.UserId);
        });

        modelBuilder.Entity<FeedbackReadModel>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_Feedbacks_CustomerId");

            entity.HasIndex(e => e.OrderId, "IX_Feedbacks_OrderId").IsUnique();

            entity.Property(e => e.FeedbackId).HasMaxLength(26);
            entity.Property(e => e.CustomerId).HasMaxLength(26);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.OrderId).HasMaxLength(26);
            entity.Property(e => e.Substance).HasMaxLength(255);

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks).HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.Order).WithOne(p => p.Feedback).HasForeignKey<FeedbackReadModel>(d => d.OrderId);
        });

        modelBuilder.Entity<OrderReadModel>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_Orders_CustomerId");

            entity.Property(e => e.OrderId).HasMaxLength(26);
            entity.Property(e => e.CustomerId).HasMaxLength(26);
            entity.Property(e => e.OrderStatus).HasMaxLength(20);
            entity.Property(e => e.OrderTransactionId).HasMaxLength(26);
            entity.Property(e => e.PaymentStatus).HasMaxLength(20);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders).HasForeignKey(d => d.CustomerId);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasIndex(e => e.OrderId, "IX_OrderDetails_OrderId");

            entity.HasIndex(e => e.ProductId, "IX_OrderDetails_ProductId");

            entity.Property(e => e.OrderDetailId).HasMaxLength(26);
            entity.Property(e => e.OrderId).HasMaxLength(26);
            entity.Property(e => e.ProductId).HasMaxLength(26);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).HasForeignKey(d => d.OrderId);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<OrderTransactionReadModel>(entity =>
        {
            entity.HasIndex(e => e.BillId, "IX_OrderTransactions_BillId");

            entity.HasIndex(e => e.OrderId, "IX_OrderTransactions_OrderId").IsUnique();

            entity.HasIndex(e => e.VoucherId, "IX_OrderTransactions_VoucherId");

            entity.Property(e => e.OrderTransactionId).HasMaxLength(26);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.BillId).HasMaxLength(26);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.OrderId).HasMaxLength(26);
            entity.Property(e => e.PayerEmail).HasMaxLength(200);
            entity.Property(e => e.PayerName).HasMaxLength(50);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.VoucherId).HasMaxLength(26);

            entity.HasOne(d => d.Bill).WithMany(p => p.OrderTransactions).HasForeignKey(d => d.BillId);

            entity.HasOne(d => d.Order).WithOne(p => p.OrderTransaction).HasForeignKey<OrderTransactionReadModel>(d => d.OrderId);

            entity.HasOne(d => d.Voucher).WithMany(p => p.OrderTransactions).HasForeignKey(d => d.VoucherId);
        });

        modelBuilder.Entity<ProductReadModel>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryId");

            entity.Property(e => e.ProductId).HasMaxLength(26);
            entity.Property(e => e.CategoryId).HasMaxLength(26);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.ProductPrice).HasPrecision(18, 2);
            entity.Property(e => e.ProductStatus).HasMaxLength(20);

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasForeignKey(d => d.CategoryId);
        });

        modelBuilder.Entity<ShippingInformationReadModel>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_ShippingInformations_CustomerId");

            entity.Property(e => e.ShippingInformationId).HasMaxLength(26);
            entity.Property(e => e.CustomerId).HasMaxLength(26);
            entity.Property(e => e.District).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
            entity.Property(e => e.Province).HasMaxLength(50);
            entity.Property(e => e.SpecificAddress).HasMaxLength(50);
            entity.Property(e => e.Ward).HasMaxLength(50);

            entity.HasOne(d => d.Customer).WithMany(p => p.ShippingInformations).HasForeignKey(d => d.CustomerId);
        });

        modelBuilder.Entity<UserReadModel>(entity =>
        {
            entity.Property(e => e.UserId).HasMaxLength(26);
            entity.Property(e => e.DateOfBirth).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.ImageUrl).HasMaxLength(256);
            entity.Property(e => e.PasswordHash).HasMaxLength(64);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.UserStatus).HasMaxLength(20);
        });

        modelBuilder.Entity<UserAuthenticationToken>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_UserAuthenticationTokens_UserId");

            entity.Property(e => e.UserAuthenticationTokenId).HasMaxLength(26);
            entity.Property(e => e.ExpiredTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.TokenType).HasMaxLength(20);
            entity.Property(e => e.UserId).HasMaxLength(26);
            entity.Property(e => e.Value).HasMaxLength(256);

            entity.HasOne(d => d.User).WithMany(p => p.UserAuthenticationTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<VerifyVerificationTokenReadModel>(entity =>
        {
            entity.HasKey(e => e.VerificationTokenId);

            entity.HasIndex(e => e.UserId, "IX_VerifyVerificationTokens_UserId");

            entity.Property(e => e.VerificationTokenId).HasMaxLength(26);
            entity.Property(e => e.CreatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ExpiredDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Temporary).HasMaxLength(64);
            entity.Property(e => e.UserId).HasMaxLength(26);

            entity.HasOne(d => d.User).WithMany(p => p.VerifyVerificationTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<VoucherReadModel>(entity =>
        {
            entity.Property(e => e.VoucherId).HasMaxLength(26);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ExpiredDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.StartDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.VoucherCode).HasMaxLength(20);
            entity.Property(e => e.VoucherName).HasMaxLength(20);
            entity.Property(e => e.VoucherType).HasMaxLength(20);
        });

        modelBuilder.Entity<WishItemReadModel>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_WishItems_CustomerId");

            entity.HasIndex(e => e.ProductId, "IX_WishItems_ProductId");

            entity.Property(e => e.WishItemId).HasMaxLength(26);
            entity.Property(e => e.CustomerId).HasMaxLength(26);
            entity.Property(e => e.ProductId).HasMaxLength(26);

            entity.HasOne(d => d.Customer).WithMany(p => p.WishItems).HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.Product).WithMany(p => p.WishItems).HasForeignKey(d => d.ProductId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
