namespace CSharpJWT.Services
{
    using System.Net.Http;
    using CSharpJWT.Common;
    using CSharpJWT.Common.Models;
    using CSharpJWT.Common.Services;
    using CSharpJWT.Common.Utilities;
    using CSharpJWT.Internal;
    using Microsoft.AspNetCore.Http;

    public class CSharpAuthenticateService : CSharpAuthenticateServiceBase, ICSharpAuthenticateService
    {
        public CSharpAuthenticateService(IHttpClientFactory clientFactory, TokenValidationOptions options) : base(clientFactory, options)
        {
        }

        public override bool ValidateIssuer(HttpContext context)
        {
            var issuer = context.User.Claims.GetValue(CSharpClaimsIdentity.IssuerClaimType);

            return Configuration.Issuer.Equals(issuer);
        }
    }
}
