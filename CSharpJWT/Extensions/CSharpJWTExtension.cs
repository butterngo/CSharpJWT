namespace CSharpJWT.Extensions
{
    using CSharpJWT.Domain;
    using CSharpJWT.Middleware;
    using CSharpJWT.Models;
    using CSharpJWT.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Net.Http;
    using System.Text;

    public static class CSharpJWTExtension
    {
        public static IApplicationBuilder AddJWTMiddleware(this IApplicationBuilder app,
            string tokenPath = "/CSharp-token",
            string revokePath = "/CSharp-revoke-token",
            int tokenExpiration = 30,
            int refreshTokenExpiration = 1440)
        {
            app.UseMiddleware<TokenProviderMiddleware>(new TokenProviderOptions
            {
                TokenPath = tokenPath,
                RevokePath = revokePath,
                Issuer = Configuration.Issuer,
                SecurityKey = Configuration.SecurityKey,
                ValidateClient = Configuration.ValidateClient,
                TokenExpiration = TimeSpan.FromMinutes(+tokenExpiration),
                RefreshTokenExpiration = TimeSpan.FromMinutes(+refreshTokenExpiration)
            });

            return app;
        }

        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.SecurityKey))
                };
            });

            return services.AddSingleton<ICSharpAuthenticateService, CSharpAuthenticateService>();
        }

        public static IServiceCollection AddCSharpIdentity<TContext>(this IServiceCollection services)
            where TContext : CSharpJWTContext
        {
            services.AddIdentity<User, Role>(Options => CSharpUserManager.CreateOptions(Options))
                .AddEntityFrameworkStores<TContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<CSharpUserManager, CSharpUserManager>();

            services.AddScoped<CSharpSignInManager, CSharpSignInManager>();

            services.AddScoped<IClientService, ClientService>();

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }

    public static class CSharpJWTServerConfiguration
    {
        public static void Init(IConfiguration configuration)
        {
            Configuration.Audience = configuration.GetValue<string>("JWTSettings:Audience");

            Configuration.Issuer = configuration.GetValue<string>("JWTSettings:Issuer");

            Configuration.ValidateClient = configuration.GetValue<bool>("JWTSettings:ValidateClient");

            Configuration.PhysicalSecretPath = configuration.GetValue<string>("JWTSettings:SecretPath");

            if (!System.IO.File.Exists(Configuration.PhysicalSecretPath))
            {
                throw new Exception($"Not found this path: {Configuration.PhysicalSecretPath}");
            }

            Configuration.SecurityKey = System.IO.File.ReadAllText(Configuration.PhysicalSecretPath);

        }
    }

    public static class CSharpJWTClientConfiguration
    {
        public static void Init(string issuer)
        {
            Configuration.Issuer = issuer;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(issuer);

                    client.Timeout = TimeSpan.FromSeconds(5);

                    var response = client.GetAsync("/oauth/secret.ssh").Result;

                    response.EnsureSuccessStatusCode();

                    Configuration.SecurityKey = response.Content.ReadAsStringAsync().Result;
                }
                catch
                {
                    throw new Exception($"Not found oauth server with host {Configuration.Issuer}");
                }
            }

        }
    }
}
