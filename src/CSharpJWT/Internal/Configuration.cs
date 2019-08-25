namespace CSharpJWT.Internal
{
    using CSharpJWT.Extensions;
    using CSharpJWT.Models;
    using Microsoft.Extensions.Configuration;
    using System;

    internal static class Configuration
    {
        public static string TokenPath { get; set; } = "/csharp-token";

        public static string RevokePath { get; set; } = "/csharp-revoke-token";

        public static string SecurityKey { get; set; }

        public static string Audience { get; set; }

        public static string Issuer { get; set; }

        public static bool ValidateClient { get; set; }

        public static bool ValidateIssuer { get; set; } = true;

        public static bool ValidateAudience { get; set; }

        /// <summary>
        /// default 30 minutes
        /// </summary>
        public static TimeSpan TokenExpiration { get; set; } = TimeSpan.FromMinutes(+30);

        /// <summary>
        /// default 1440 minutes
        /// </summary>
        public static TimeSpan RefreshTokenExpiration { get; set; } = TimeSpan.FromMinutes(+1440);

        public static TokenProviderOptions Init(IConfiguration configuration)
        {
            configuration.ToConfiguration();

            return new TokenProviderOptions
            {
                TokenPath = TokenPath,
                RevokePath = RevokePath,
                Issuer = Issuer,
                SecurityKey = SecurityKey,
                ValidateClient = ValidateClient,
                TokenExpiration = TokenExpiration,
                RefreshTokenExpiration = RefreshTokenExpiration
            };
        }

        private static void ToConfiguration(this IConfiguration configuration)
        {
            Audience = configuration.TryGetValue<string>("CSharpJWT:Audience");

            Issuer = configuration.TryGetValue<string>("CSharpJWT:Issuer");

            ValidateClient = configuration.TryGetValue<bool>("CSharpJWT:ValidateClient");

            SecurityKey = configuration.TrySecurityKey("CSharpJWT:SecurityKey");
        }
    }
}
