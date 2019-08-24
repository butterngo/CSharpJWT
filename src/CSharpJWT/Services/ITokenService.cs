namespace CSharpJWT.Internal.Services
{
    using CSharpJWT.Models;
    using Microsoft.AspNetCore.Authentication;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITokenService
    {
        Task<IDictionary<string, object>> GenerateTokenAsync(TokenRequest dto);

        Task<RefreshTokenResult> VerifyRefreshTokenAsync(string token);

        Task<bool> RevokeTokenAsync(AuthenticateResult authenticateResult);

    }
}
