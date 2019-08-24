namespace CSharpJWT.Extensions
{
    using CSharpJWT.Internal.Caches;
    using Microsoft.Extensions.Caching.SqlServer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class CacheExtension
    {
        public static IServiceCollection AddCSharpJWTDistributedMemoryCache(this IServiceCollection services)
        {
            return services
                .AddDistributedMemoryCache()
                .AddSingleton<JWTTokenDistributedCache>();
        }

        public static IServiceCollection AddCSharpJWTDistributedSqlServerCache(this IServiceCollection services,
            Action<SqlServerCacheOptions> setupAction)
        {
            return services
                .AddDistributedSqlServerCache(setupAction)
                .AddSingleton<JWTTokenDistributedCache>();
        }
    }
}
