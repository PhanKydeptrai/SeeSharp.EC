using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL.ReadModels;

namespace Persistence.Database.PostgreSQL;

public sealed class NextSharpPostgreSQLReadDbContextSample : DbContext
{
    public NextSharpPostgreSQLReadDbContextSample(DbContextOptions<NextSharpPostgreSQLReadDbContextSample> options) : base(options) { }

    public DbSet<BillReadModel> BillReadModels { get; set; }
    public DbSet<CategoryReadModel> CategorieReadModels { get; set; }
    public DbSet<CustomerReadModel> CustomerReadModels { get; set; }
    public DbSet<CustomerVoucherReadModel> CustomerVoucherReadModels { get; set; }
    public DbSet<EmployeeReadModel> EmployeeReadModels { get; set; }
    public DbSet<FeedbackReadModel> FeedbackReadModels { get; set; }
    public DbSet<OrderDetailReadModel> OrderDetails { get; set; }
    public DbSet<OrderReadModel> OrderReadModels { get; set; }
    public DbSet<OrderTransactionReadModel> OrderTransactionReadModels { get; set; }
    public DbSet<ProductReadModel> ProductReadModels { get; set; }
    public DbSet<ShippingInformationReadModel> ShippingInformationReadModels { get; set; }
    public DbSet<UserAuthenticationTokenReadModel> UserAuthenticationTokenReadModels { get; set; }
    public DbSet<UserReadModel> UserReadModels { get; set; }
    public DbSet<VerifyVerificationTokenReadModel> VerifyVerificationTokenReadModels { get; set; }
    public DbSet<VoucherReadModel> VoucherReadModels { get; set; }
    public DbSet<WishItemReadModel> WishItemReadModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(NextSharpPostgreSQLReadDbContextSample).Assembly,
            WriteConfigurationsFilter);

        base.OnModelCreating(modelBuilder);
    }
    private static bool WriteConfigurationsFilter(Type type) =>
        type.FullName?.Contains("Database.PostgreSQL.Configurations.Read") ?? false;
}
