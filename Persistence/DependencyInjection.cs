using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using Domain.OutboxMessages.Services;
using Infrastructure.Repositories.CategoryRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;
using Persistence.Outbox;
using Persistence.Repositories;
using Persistence.Repositories.CategoryRepositories;

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
            .AddAbstractsDatabase(configuration)
            .AddOutBoxMessageServices();

        return services;
    }
    //Add Repository
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<CategoryRepository>();
        services.AddScoped<ICategoryRepository>(provider =>
        {
            var categoryRepository = provider.GetRequiredService<CategoryRepository>();
            return new CategoryRepositoryCached(categoryRepository, provider.GetService<IDistributedCache>()!);
        });

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
        services.AddDbContext<NextSharpMySQLReadDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString)
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
        services.AddDbContext<NextSharpPostgreSQLReadDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        return services;
    }

    //FIXME: Add Abstracts Database
    //Add Abstracts Replica Database
    private static IServiceCollection AddAbstractsDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddScoped<INextSharpReadOnlyDbContext>(provider => provider.GetRequiredService<NextSharpReadOnlyDbContext>());
        //services.AddScoped<INextSharpDbContext>(provider => provider.GetRequiredService<NextSharpDbContext>());
        return services;
    }


}
