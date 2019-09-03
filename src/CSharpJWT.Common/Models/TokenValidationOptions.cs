namespace CSharpJWT.Common.Models
{
    using Microsoft.IdentityModel.Tokens;
    using System.Text;

    public class TokenValidationOptions
    {
        public TokenValidationOptions()
        {
            Scopes = new string[] { };
        }

        public TokenValidationOptions(string issuer, string securityKey):this()
        {
            Issuer = issuer;

            if (!string.IsNullOrEmpty(securityKey))
            {
                IssuerSigningKey = GenerateIssuerSigningKey(securityKey);
            }
        }

        public string Issuer { get; set; }
        public SymmetricSecurityKey IssuerSigningKey { get; set; }
        public string[] Scopes { get; set; }

        public SymmetricSecurityKey GenerateIssuerSigningKey(string value)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(value));
        }

        public TokenValidationParameters GenerateTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Issuer,
                IssuerSigningKey = IssuerSigningKey
            };
        }
    }
}
