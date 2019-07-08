using System;

namespace CSharpJWT.Models
{
    public class RefreshTokenResult
    {
        public RefreshTokenResult()
        {
            Successed = true;
        }

        public RefreshTokenResult(object error)
        {
            Successed = false;
            Error = error;
        }

        public RefreshTokenResult(string clientId, string userId)
        {
            Successed = true;
            UserId = userId;
            ClientId = clientId;
        }

        public RefreshTokenResult(string clientId, string userId, DateTime expirationDate)
        {
            Successed = true;
            UserId = userId;
            ClientId = clientId;
            ExpirationDate = expirationDate;
        }
        public bool Successed { get; set; }
        public string UserId { get; set; }
        public string ClientId { get; set; }
        public object Error { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
