namespace CSharpJWT.Middleware
{
    using CSharpJWT.Internal.Caches;
    using CSharpJWT.Extensions;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class CacheHandler
    {
        public async Task ExecuteAsync(HttpContext context)
        {
            var cache = (JWTTokenDistributedCache)context.RequestServices.GetService(typeof(JWTTokenDistributedCache));

            string key = context.Request.Query["key"];

            await context.OkAsync(await cache.GetAsync(key));
        }

    }
}
