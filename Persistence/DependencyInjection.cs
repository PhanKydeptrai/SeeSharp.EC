using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;

namespace Persistence;
//FIXME: AddPersistnce
public static class DependencyInjection
{
    public static IServiceCollection AddPersistnce(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPrimaryDatabase(configuration)
            .AddReadOnlyDatabase(configuration)
            .AddAbstractsDatabase(configuration);

        return services;
    }

    //Add Primary Database(MySQL)
    private static IServiceCollection AddPrimaryDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        string connectionString = configuration.GetConnectionString("PrimaryDatabase")
            ?? throw new Exception("MySQL connection string is null");
        services.AddDbContext<NextSharpMySQLDbContext>(options =>
        {
            options.UseMySQL(connectionString);
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
