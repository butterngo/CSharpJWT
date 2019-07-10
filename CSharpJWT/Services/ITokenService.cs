namespace CSharpJWT.Services
{
    using CSharpJWT.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITokenService
    {
        Task<IDictionary<string, object>> GenerateTokenAsync(TokenRequest dto);

        Task<RefreshTokenResult> VerifyRefreshTokenAsync(string token);

        Task<bool> IsValidTokenAsync(HttpContext context);

        bool IsValidToken(HttpContext context);

        Task<bool> RevokeTokenAsync(AuthenticateResult authenticateResult);

    }
}
