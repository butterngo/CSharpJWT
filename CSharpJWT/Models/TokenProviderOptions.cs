namespace CSharpJWT.Models
{
    using System;
    
    public class TokenProviderOptions
    {
        public string TokenPath { get; set; }

        public string RevokePath { get;set; }

        public string Issuer { get; set; }

        public bool ValidateClient { get; set; } = false;

        public TimeSpan TokenExpiration { get; set; }

        public TimeSpan RefreshTokenExpiration { get; set; }

        public string SecurityKey { get; set; }
    }
}
