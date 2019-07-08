namespace CSharpJWT.Models
{
    using System;
    using System.Collections.Generic;

    public class TokenRequest
    {
        public TokenRequest() { }

        public TokenRequest(TokenProviderOptions options):this(options, new List<CustomClaim>()) { }

        public TokenRequest(TokenProviderOptions options, List<CustomClaim> claims)
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
        public List<CustomClaim> Claims { get; set; }
        public TimeSpan TokenExpiration { get; set; }
        public TimeSpan RefreshTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
        public IDictionary<string, object> Responses { get; set; }
    }

    public class CustomClaim
    {
        public CustomClaim(string type, object value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; set; }
        public object Value { get; set; }
    }
}
