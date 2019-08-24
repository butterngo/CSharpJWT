using Microsoft.AspNetCore.Http;
namespace CSharpJWT.Extensions
{
    using System.Threading.Tasks;

    public static class HttpContextExtension
    {
        public static async Task UnauthorizedAsync(this HttpContext context)
        {
            context.Response.StatusCode = 401;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync("Unauthorized");
        }

        public static async Task BadRequestAsync(this HttpContext context, object msg)
        {
            context.Response.StatusCode = 400;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(msg.ToJson());
        }

        public static async Task OkAsync(this HttpContext context, object msg)
        {
            context.Response.StatusCode = 200;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(msg.ToJson());
        }

        public static async Task OkAsync(this HttpContext context, string msg)
        {
            context.Response.StatusCode = 200;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(msg);
        }
    }
}
