namespace CSharpJWT.Common.Models
{
    using System;

    public class TokenInformation
    {
        public TokenInformation() { }

        public TokenInformation(string userId, string clientId, string token, DateTime expirationDate)
        {
            UserId = userId;
            ClientId = clientId;
            Token = token;
            ExpirationDate = expirationDate;
        }

        public string UserId { get; set; }

        public string ClientId { get; set; }

        public string Token { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
