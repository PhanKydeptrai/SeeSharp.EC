using Domain.Entities.Bills;
using Domain.Entities.BillDetails;
using Domain.Entities.Categories;
using Domain.Entities.Customers;
using Domain.Entities.CustomerVouchers;
using Domain.Entities.Employees;
using Domain.Entities.Feedbacks;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants; 
using Domain.Entities.ShippingInformations;
using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;
using Domain.Entities.VerificationTokens;
using Domain.Entities.Vouchers;
using Domain.Entities.WishItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt.Extensions;
using Persistence.Database.PostgreSQL.Configurations.Write;
using SharedKernel;

namespace Persistence.Database.PostgreSQL;

public sealed class SeeSharpPostgreSQLWriteDbContext : DbContext
{   
    public UserName RootUserName { get; }
    public Email RootUserEmail { get; }
    public PhoneNumber RootUserPhoneNumber { get; }
    public PasswordHash RootUserPassword { get; }
    
    public SeeSharpPostgreSQLWriteDbContext(DbContextOptions<SeeSharpPostgreSQLWriteDbContext> options, IConfiguration configuration)
        : base(options)
    { 
        RootUserName = UserName.FromString(configuration["RootUser:UserName"]!);
        RootUserEmail = Email.FromString(configuration["RootUser:Email"]!);
        RootUserPhoneNumber = PhoneNumber.FromString(configuration["RootUser:PhoneNumber"]!);
        RootUserPassword = PasswordHash.FromString(configuration["RootUser:Password"]!.SHA256());
    }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<BillDetail> BillDetails { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerVoucher> CustomerVouchers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderTransaction> OrderTransactions { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<ShippingInformation> ShippingInformations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<VerificationToken> VerificationTokens { get; set; }
    public DbSet<UserAuthenticationToken> UserAuthenticationTokens { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }
    public DbSet<WishItem> WishItems { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SeeSharpPostgreSQLWriteDbContext).Assembly,
            WriteConfigurationsFilter);
        
        modelBuilder.ApplyConfiguration(new UserConfigurationForPostgreSQL(RootUserName, RootUserEmail, RootUserPhoneNumber, RootUserPassword));
        base.OnModelCreating(modelBuilder);
    }

    private static bool WriteConfigurationsFilter(Type type) =>
        type.FullName?.Contains("Database.PostgreSQL.Configurations.Write") ?? false;
}
