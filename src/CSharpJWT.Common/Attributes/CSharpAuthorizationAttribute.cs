namespace CSharpJWT.Authentication
{
    using CSharpJWT.Common.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class CSharpAuthorizeAttribute : TypeFilterAttribute
    {

        public CSharpAuthorizeAttribute(string[] Roles = null)
            : base(typeof(CSharpAuthorizeFilter))
        {
            Roles = Roles == null ? new string[] { } : Roles;

            Arguments = new object[] { Roles };
        }
    }

    public class CSharpAuthorizeFilter : IAuthorizationFilter
    {
        private readonly string[] _roles;

        public CSharpAuthorizeFilter(string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authenticateService = (ICSharpAuthenticateService)context
                .HttpContext
                .RequestServices
                .GetService(typeof(ICSharpAuthenticateService));

            if (!authenticateService.ValidateIssuer(context.HttpContext))
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                return;
            }

            if (!authenticateService.ValidateRole(context.HttpContext, _roles))
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                return;
            }

            if (!authenticateService.ValidateToken(context.HttpContext))
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
                return;
            }

        }

    }
}
