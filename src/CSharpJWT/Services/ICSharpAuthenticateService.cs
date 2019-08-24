namespace CSharpJWT.Services
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICSharpAuthenticateService
    {
        bool ValidateAudience(HttpContext context, List<string> audiences);

        bool ValidateRole(HttpContext context, IEnumerable<string> roles);

        bool ValidateIssuer(HttpContext context);

        Task<bool> ValidateTokenAsync(HttpContext context);

        bool ValidateToken(HttpContext context);
    }
}
