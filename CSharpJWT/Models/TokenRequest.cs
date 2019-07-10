namespace CSharpJWT.Models
{
    using CSharpJWT.Domain;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;

    public class TokenRequest
    {
        public TokenRequest() { }

        public TokenRequest(TokenProviderOptions options):this(options, new List<Claim>()) { }

        public TokenRequest(TokenProviderOptions options, List<Claim> claims)
        {
            Issuer = options.Issuer;
            Audience = options.Audience;
            Claims = claims;
            TokenExpiration = options.TokenExpiration;
            RefreshTokenExpiration = options.RefreshTokenExpiration;
            SecurityKey = options.SecurityKey;
            Responses = new Dictionary<string, object>();
        }

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public User User { get; set; }
        public Client Client { get; set; }
        public List<Claim> Claims { get; set; }
        public TimeSpan TokenExpiration { get; set; }
        public TimeSpan RefreshTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
        public IDictionary<string, object> Responses { get; set; }
    }
}
