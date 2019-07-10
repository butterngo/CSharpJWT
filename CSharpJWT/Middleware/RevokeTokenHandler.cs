namespace CSharpJWT.Middleware
{
    using CSharpJWT.Extensions;
    using CSharpJWT.Services;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;
    
    public class RevokeTokenHandle
    {
        public async Task ExecuteAsync(HttpContext context)
        {

            var authenticateResult = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
            {
                await Unauthorized(context);

                return;
            }

            if (!context.Request.Method.Equals("POST"))
            {
                await BadRequest(context, new { error = "Bad request.." });

                return;
            }

            var tokenService = (ITokenService)context.RequestServices.GetService(typeof(ITokenService));

            var authenticationService = (IAuthenticationService)context.RequestServices.GetService(typeof(IAuthenticationService));

            if (await tokenService.RevokeTokenAsync(authenticateResult))
            {
                context.Response.StatusCode = 200;

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync("Succeeded");
            }
            else
            {
                await Unauthorized(context);
            }

        }

        private async Task BadRequest(HttpContext context, object msg)
        {
            context.Response.StatusCode = 400;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(msg.ToJson());
        }

        private async Task Unauthorized(HttpContext context)
        {
            context.Response.StatusCode = 401;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync("Unauthorized");
        }
    }
}
