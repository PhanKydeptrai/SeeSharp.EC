using Microsoft.EntityFrameworkCore;
using Domain.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.PostgreSQL;

public sealed class NextSharpPostgreSQLReadDbContext : DbContext
{
    public NextSharpPostgreSQLReadDbContext(DbContextOptions<NextSharpPostgreSQLReadDbContext> options)
        : base(options) { }
    public DbSet<BillReadModel> Bills { get; set; }
    public DbSet<CategoryReadModel> Categories { get; set; }
    public DbSet<CustomerReadModel> Customers { get; set; }
    public DbSet<CustomerVoucherReadModel> CustomerVouchers { get; set; }
    public DbSet<EmployeeReadModel> Employees { get; set; }
    public DbSet<FeedbackReadModel> Feedbacks { get; set; }
    public DbSet<OrderDetailReadModel> OrderDetails { get; set; }
    public DbSet<OrderReadModel> Orders { get; set; }
    public DbSet<OrderTransactionReadModel> OrderTransactions { get; set; }
    public DbSet<ProductReadModel> Products { get; set; }
    public DbSet<ShippingInformationReadModel> ShippingInformations { get; set; }
    // public DbSet<UserAuthenticationTokenReadModel> UserAuthenticationTokens { get; set; }
    public DbSet<UserReadModel> Users { get; set; }
    // public DbSet<VerificationTokenReadModel> VerificationTokens { get; set; }
    public DbSet<VoucherReadModel> Vouchers { get; set; }
    public DbSet<WishItemReadModel> WishItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(NextSharpPostgreSQLReadDbContext).Assembly,
            WriteConfigurationsFilter);

        base.OnModelCreating(modelBuilder);
    }
    private static bool WriteConfigurationsFilter(Type type) =>
        type.FullName?.Contains("Database.PostgreSQL.Configurations.Read") ?? false;
}