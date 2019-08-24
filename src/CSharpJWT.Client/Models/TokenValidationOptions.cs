namespace CSharpJWT.Client.Models
{
    using Microsoft.IdentityModel.Tokens;
    using System.Text;

    public class TokenValidationOptions
    {
        public string Issuer
        {
            get
            {
                return Configuration.Issuer;
            }
            set
            {
                Configuration.Issuer = value;
            }
        }

        public string SecurityKey
        {
            get
            {
                return Configuration.SecurityKey;
            }
            set
            {
                Configuration.SecurityKey = value;
            }
        }

        public SymmetricSecurityKey IssuerSigningKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));

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
