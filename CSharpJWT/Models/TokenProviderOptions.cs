namespace CSharpJWT.Models
{
    using System;
    
    public class TokenProviderOptions
    {
        public TokenProviderOptions() { }

        public string Path { get; set; } = "/CSharp-token";

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public bool VerifyClient { get; set; } = false;

        public TimeSpan TokenExpiration { get; set; }
        public TimeSpan RefreshTokenExpiration { get; set; }
        public string SecurityKey { get; set; } = "CSharp.OAuthServices";
    }
}
