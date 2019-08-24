namespace CSharpJWT.Models
{
    using CSharpJWT.Internal;
    using System;
    
    public class TokenProviderOptions
    {
        /// <summary>
        /// default path is "/csharp-token"
        /// </summary>
        public string TokenPath
        {
            get
            {
                return Configuration.TokenPath;
            }
            set
            {
                Configuration.TokenPath = value;
            }
        }

        /// <summary>
        /// default path is "/csharp-revoke-token"
        /// </summary>
        public string RevokePath
        {
            get
            {
                return Configuration.RevokePath;
            }
            set
            {
                Configuration.RevokePath = value;
            }
        }

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

        public bool ValidateClient
        {
            get
            {
                return Configuration.ValidateClient;
            }
            set
            {
                Configuration.ValidateClient = value;
            }
        }

        public bool ValidateAudience
        {
            get
            {
                return Configuration.ValidateAudience;
            }
            set
            {
                Configuration.ValidateAudience = value;
            }
        }

        /// <summary>
        /// default 30 minutes
        /// </summary>
        public TimeSpan TokenExpiration
        {
            get
            {
                return Configuration.TokenExpiration;
            }
            set
            {
                Configuration.TokenExpiration = value;
            }
        }

        /// <summary>
        /// default 1440 minutes
        /// </summary>
        public TimeSpan RefreshTokenExpiration
        {
            get
            {
                return Configuration.RefreshTokenExpiration;
            }
            set
            {
                Configuration.RefreshTokenExpiration = value;
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
    }
}
