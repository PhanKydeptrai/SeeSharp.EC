using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using Domain.IRepositories.Customers;
using Domain.IRepositories.Employees;
using Domain.IRepositories.Orders;
using Domain.IRepositories.Products;
using Domain.IRepositories.UserAuthenticationTokens;
using Domain.IRepositories.Users;
using Domain.IRepositories.VerificationTokens;
using Domain.IRepositories.Vouchers;
using Domain.IRepositories.WishItems;
using Domain.OutboxMessages.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database.PostgreSQL;
using Persistence.Outbox;
using Persistence.Repositories;
using Persistence.Repositories.CategoryRepositories;
using Persistence.Repositories.CustomerRepositories;
using Persistence.Repositories.EmployeeRepositories;
using Persistence.Repositories.OrderRepositories;
using Persistence.Repositories.ProductRepositories;
using Persistence.Repositories.UserAuthenticationTokenRepositories;
using Persistence.Repositories.UserRepositories;
using Persistence.Repositories.VerificationTokenRepositories;
using Persistence.Repositories.WishItemRepositories;
using Persistence.Repositories.VoucherRepositories;
using Persistence.Repositories.ShippingInformationRepositories;
using Domain.IRepositories.ShippingInformations;
using Domain.IRepositories.Bills;
using Persistence.Repositories.BillRepositories;
using Persistence.Repositories.FeedbackRepositories;
using Domain.IRepositories.Feedbacks;

namespace Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistnce(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabase(configuration)
            .AddRepository()
            .AddOutBoxMessageServices();

        return services;
    }
    //Add Repository
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVerificationTokenRepository, VerificationTokenRepository>();
        services.AddScoped<IUserAuthenticationTokenRepository, UserAuthenticationTokenRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IWishItemRepository, WishItemRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<IShippingInformationRepository, ShippingInformationRepository>();
        services.AddScoped<IBillRepository, BillRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    //Add OutBoxMessageServices
    public static IServiceCollection AddOutBoxMessageServices(this IServiceCollection services)
    {
        services.AddScoped<IOutBoxMessageServices, OutBoxMessageServices>();
        return services;
    }
    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        string connectionString = configuration.GetConnectionString("PrimaryDatabase")
            ?? throw new Exception("PostgreSQL connection string is null");
        //Dbcontext để ghi
        services.AddDbContext<SeeSharpPostgreSQLWriteDbContext>((options) =>
        {
            options.UseNpgsql(connectionString);
        });

        //Dbcontext để đọc
        services.AddDbContext<SeeSharpPostgreSQLReadDbContext>((options) =>
        {
            options.UseNpgsql(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }
}
