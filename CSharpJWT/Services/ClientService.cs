namespace CSharpJWT.Services
{
    using CSharpJWT.Domain;
    using CSharpJWT.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class ClientService : IClientService
    {
        protected DbSet<Client> Clients { get; private set; }

        private const string InvalidSecretKey = "Invalid secret key.";

        public ClientService(CSharpJWTContext context)
        {
            Clients = context.Clients;
        }

        public async Task<Client> FindByClientIdAsync(string clientId)
        {
            var client = await Clients.FirstOrDefaultAsync(x => x.ClientId.Equals(clientId));

            return client;
        }

        public async Task<ClientResult> VerifyClientAsync(HttpContext context)
        {
            
            try
            {
                var headers = context.Request.Headers;

                if (!string.IsNullOrEmpty(headers["Authorization"]))
                {
                    var array = headers["Authorization"].ToString().Split(' ');

                    var credentials = Base64Decode(array[1]).Split(':');

                    string clientId = credentials[0];

                    string secret = credentials[1];

                    var result = await Clients.SingleOrDefaultAsync(x => x.ClientId.Equals(clientId) && x.Secret.Equals(secret));

                    if(result == null ) return new ClientResult(new { error = InvalidSecretKey });

                    return new ClientResult(result.Id);
                }
                else
                {
                    return new ClientResult(new { error = InvalidSecretKey });
                }
            }
            catch
            {
                return new ClientResult(new { error = InvalidSecretKey });
            } 
        }

        public async Task<string> GetSecretKeyByClientIdAsync(string clientId)
        {
            var client = await Clients.FirstOrDefaultAsync(x => x.ClientId.Equals(clientId));

            if (client == null) throw new Exception($"Not found clientId {clientId}");

            return Base64Encode($"{clientId}:{client.Secret}");
        }

        protected virtual string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);

            return Convert.ToBase64String(plainTextBytes);
        }

        protected virtual string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);

            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
