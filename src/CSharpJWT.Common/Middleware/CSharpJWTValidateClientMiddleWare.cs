namespace CSharpJWT.Common.MiddleWare
{
    using CSharpJWT.Common.Models;
    using CSharpJWT.Common.Services;
    using CSharpJWT.Common.Utilities;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using System.Linq;
    using System.Threading.Tasks;

    public class CSharpJWTValidateClientMiddleware
    {
        public readonly RequestDelegate _next;

        public readonly TokenValidationOptions _options;

        public CSharpJWTValidateClientMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var authenticateResult = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            var authenticateService = context
                .RequestServices
                .GetService(typeof(ICSharpAuthenticateService)) 
                as ICSharpAuthenticateService;

            if (authenticateResult.Succeeded)
            {
                var clientId = context.User.Claims.GetValue(CSharpClaimsIdentity.ClientIdClaimType);

                if (!string.IsNullOrEmpty(clientId))
                {
                    if (!authenticateService.ValidateScope(context))
                    {
                        await context.UnauthorizedAsync();

                        return;
                    }
                } 
            }

            await _next(context);
        }
    }
}
