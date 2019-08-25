namespace CSharpJWT.Attributes
{
    using CSharpJWT.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Linq;

    public class CSharpAuthorizeAttribute : TypeFilterAttribute
    {

        public CSharpAuthorizeAttribute(string[] Audiences = null, string[] Roles = null)
            : base(typeof(CSharpAuthorizeFilter))
        {
            Audiences = Audiences == null ? new string[] { } : Audiences;

            Roles = Roles == null ? new string[] { } : Roles;

            Arguments = new object[] { Audiences, Roles };
        }
    }

    public class CSharpAuthorizeFilter : IAuthorizationFilter
    {
        private readonly string[] _audiences;

        private readonly string[] _roles;

        public CSharpAuthorizeFilter(string[] audiences, string[] roles)
        {
            _audiences = audiences;

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

            if (!authenticateService.ValidateAudience(context.HttpContext, _audiences.ToList()))
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
