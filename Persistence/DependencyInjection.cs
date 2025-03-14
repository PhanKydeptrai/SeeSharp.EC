using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.IRepositories.CategoryRepositories;
using Domain.OutboxMessages.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;
using Persistence.Outbox;
using Persistence.Repositories;
using Persistence.Repositories.CategoryRepositories;
using Persistence.Repositories.ProductRepositories;
using Domain.IRepositories.Customers;
using Persistence.Repositories.CustomerRepositories;
using Persistence.Repositories.UserRepositories;
using Domain.IRepositories.Users;
using Domain.IRepositories.VerificationTokens;
using Persistence.Repositories.VerificationTokenRepositories;
using Domain.IRepositories.UserAuthenticationTokens;
using Persistence.Repositories.UserAuthenticationTokenRepositories;
using Domain.IRepositories.Orders;
using Persistence.Repositories.OrderRepositories;
using Domain.IRepositories.WishItems;
using Persistence.Repositories.WishItemRepositories;

namespace Persistence;
//FIXME: AddPersistnce
public static class DependencyInjection
{
    public static IServiceCollection AddPersistnce(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPrimaryDatabase(configuration)
            .AddRepository()
            .AddReadOnlyDatabase(configuration)
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
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    //Add OutBoxMessageServices
    public static IServiceCollection AddOutBoxMessageServices(this IServiceCollection services)
    {
        services.AddScoped<IOutBoxMessageServices, OutBoxMessageServices>();
        return services;
    }
    //Add Primary Database(MySQL)
    private static IServiceCollection AddPrimaryDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        string connectionString = configuration.GetConnectionString("PrimaryDatabase")
            ?? throw new Exception("MySQL connection string is null");

        //Dbcontext để ghi
        services.AddDbContext<NextSharpMySQLWriteDbContext>(options =>
        {
            options.UseMySQL(connectionString);
        });

        //Dbcontext để đọc
        services.AddDbContext<NextSharpMySQLReadDbContext>((options) =>
        {
            options.UseMySQL(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }

    //Add ReadOnly Database(PostgreSQL)
    private static IServiceCollection AddReadOnlyDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        string connectionString = configuration.GetConnectionString("ReadOnlyDatabase")
            ?? throw new Exception("PostgreSQL connection string is null");
        //Dbcontext để ghi
        services.AddDbContext<NextSharpPostgreSQLWriteDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString);
        });

        //Dbcontext để đọc
        services.AddDbContext<NextSharpPostgreSQLReadDbContext>((options) =>
        {
            options.UseNpgsql(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        return services;
    }
}
