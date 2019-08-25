namespace CSharpJWT.Extensions
{
    using CSharpJWT.Internal.Caches;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Caching.SqlServer;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class CacheExtension
    {
        public static IServiceCollection AddCSharpJWTDistributedMemoryCache(this IServiceCollection services,
            Action<MemoryDistributedCacheOptions> setupAction = null)
        {
            if(setupAction == null) services.AddDistributedMemoryCache();

            else services.AddDistributedMemoryCache(setupAction);

            return services
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
