namespace CSharpJWT.Middleware
{
    using CSharpJWT.Caches;
    using CSharpJWT.Extensions;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class CacheHandler
    {
        public async Task ExecuteAsync(HttpContext context)
        {
            var cache = (JWTTokenDistributedCache)context.RequestServices.GetService(typeof(JWTTokenDistributedCache));

            string key = context.Request.Query["key"];

            var tokenInformation = await cache.GetAsync(key);

            context.Response.StatusCode = 200;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(tokenInformation.ToJson());
        }
    }
}
