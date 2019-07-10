namespace CSharpJWT.Attributes
{
    using CSharpJWT.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class CSharpAuthorizationAttribute : TypeFilterAttribute
    {
        public CSharpAuthorizationAttribute() : base(typeof(CSharpAuthorizationFilter))
        {
        }
    }

    public class CSharpAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenService = (ITokenService)context
                .HttpContext
                .RequestServices
                .GetService(typeof(ITokenService));

            if (!tokenService.IsValidToken(context.HttpContext))
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized");
            } 
        }
    }
}
