namespace CSharpJWT.Services
{
    using CSharpJWT.Domain;
    using CSharpJWT.Models;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IClientService
    {
        Task<Client> FindByClientIdAsync(string clientId);

        Task<ClientResult> VerifyClientAsync(HttpContext context);

        Task<string> GetSecretKeyByClientIdAsync(string clientId);
    }
}
