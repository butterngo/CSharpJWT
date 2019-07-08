namespace CSharpJWT.Services
{
    using CSharpJWT.Domain;
    using System.Threading.Tasks;

    public interface IClientService
    {
        Task<Client> FindByClientIdAsync(string clientId);

        Task<string> VerifyClientAsync(string secretKey);

        Task<string> GetSecretKeyByClientId(string clientId);
    }
}
