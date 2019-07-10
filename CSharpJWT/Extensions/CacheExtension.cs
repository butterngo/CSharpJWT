namespace CSharpJWT.Extensions
{
    using CSharpJWT.Caches;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class CacheExtension
    {
        public static IServiceCollection AddCache(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = configuration.GetConnectionString("DistCache_ConnectionString");
                options.SchemaName = "dbo";
                options.TableName = "TestCache";
            });

            services.AddSingleton<JWTTokenDistributedCache>();

            return services;
        }
    }
}
