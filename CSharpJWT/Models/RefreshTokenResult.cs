namespace CSharpJWT.Models
{
    using System;

    public class RefreshTokenResult
    {
        public RefreshTokenResult()
        {
            Succeeded = true;
        }

        public RefreshTokenResult(object error)
        {
            Succeeded = false;
            Error = error;
        }

        public RefreshTokenResult(string clientId, string userId)
        {
            Succeeded = true;
            UserId = userId;
            ClientId = clientId;
        }

        public bool Succeeded { get; set; }
        public string UserId { get; set; }
        public string ClientId { get; set; }
        public object Error { get; set; }
    }
}
