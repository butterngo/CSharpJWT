namespace CSharpJWT.Services
{
    using CSharpJWT.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITokenService
    {
        Task<IDictionary<string, object>> GenerateTokenAsync(TokenRequest dto);

        Task<RefreshTokenResult> VerifyRefreshTokenAsync(string token);

    }
}
