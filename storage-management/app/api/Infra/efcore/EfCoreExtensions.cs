using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace storageapi.Infra.efcore
{
    public static class EfCoreExtensions
    {
        public static IServiceCollection AddSqlDatabaseServer(this IServiceCollection services, Action<SqlDbOptions> sqlOptions)
        {
            services.Configure(sqlOptions);
            services.AddDbContext<StorageDbContext>((services, options) =>
            {
                var sqlServerOptions = services.GetRequiredService<IOptions<SqlDbOptions>>();
                options.UseSqlServer(sqlServerOptions.Value.ConnectionString
                    //, optionBuilder => optionBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                    );

                if (sqlServerOptions.Value.EnableDetailedErrors)
                {
                    options.LogTo(Console.WriteLine, LogLevel.Information);
                    options.EnableDetailedErrors();
                }
            }, ServiceLifetime.Transient, ServiceLifetime.Transient);

            return services;
        }
    }
}
