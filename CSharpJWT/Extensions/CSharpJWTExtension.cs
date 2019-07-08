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
    using System.Text;

    public static class CSharpJWTExtension
    {
        public static IApplicationBuilder AddJWTMiddleware(this IApplicationBuilder app,
            IConfiguration configuration,
            string path = "/CSharp-token",
            int tokenExpiration = 20,
            int refreshTokenExpiration = 1440)
        {
            app.UseMiddleware<TokenProviderMiddleware>(new TokenProviderOptions
            {
                Path = path,
                Audience = configuration.GetValue<string>("JWTSettings:Audience"),
                Issuer = configuration.GetValue<string>("JWTSettings:Issuer"),
                SecurityKey = GetsecretKey(),
                VerifyClient = configuration.GetValue<bool>("JWTSettings:VerifyClient"),
                TokenExpiration = TimeSpan.FromSeconds(+tokenExpiration),
                RefreshTokenExpiration = TimeSpan.FromMinutes(+refreshTokenExpiration)
            });

            return app;
        }

        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services,
            IConfiguration configuration)
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
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetValue<string>("JWTSettings:Issuer"),
                    ValidAudience = configuration.GetValue<string>("JWTSettings:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetsecretKey()))
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = async (AuthenticationFailedContext) => 
                    {
                        //TODO Some logic
                    },
                    OnChallenge = async (JwtBearerChallengeContext) => 
                    {
                        //TODO Some logic
                    },
                    OnMessageReceived = async (MessageReceivedContext) => 
                    {
                        //TODO Some logic
                    },
                    OnTokenValidated = async (TokenValidatedContext) => 
                    {
                        //TODO Some logic
                    },
                };
            });

            return services;
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

        private static string GetsecretKey()
        {
            string path = $"{AppContext.BaseDirectory}\\secret.ssh";

            string secretKey = Configuration.SecurityKey;

            if (System.IO.File.Exists(path))
            {
                secretKey = System.IO.File.ReadAllText($"{AppContext.BaseDirectory}\\secret.ssh");
            }

            return secretKey;
        }
    }
}
