namespace CSharpJWT.Middleware
{
    using CSharpJWT.Extensions;
    using CSharpJWT.Internal.Services;
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
                await context.UnauthorizedAsync();

                return;
            }

            if (!context.Request.Method.Equals("POST"))
            {
                await context.BadRequestAsync(new { error = "Bad request.." });

                return;
            }

            var tokenService = (ITokenService)context.RequestServices.GetService(typeof(ITokenService));

            var authenticationService = (IAuthenticationService)context.RequestServices.GetService(typeof(IAuthenticationService));

            if (await tokenService.RevokeTokenAsync(authenticateResult))
            {
                await context.OkAsync("Succeeded");
            }
            else
            {
                await context.UnauthorizedAsync();
            }

        }
    }
}
