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
    using Polly;
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

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

            //Configuration.PhysicalSecretPath = $"{AppDomain.CurrentDomain.BaseDirectory}//{configuration.GetValue<string>("JWTSettings:SecretPath")}";

            //if (!System.IO.File.Exists(Configuration.PhysicalSecretPath))
            //{
            //    throw new Exception($"Not found this path: {Configuration.PhysicalSecretPath}");
            //}

            Configuration.SecurityKey = "ssh-rsa AAAAB3NzaC1yc2EAAAABJQAAAQBfjJRNnuKWed3Yyy+AHYwtx0o0cK4wnKSM3TQ5r5Fv5PMt3ZUndwinlagR5IvFYT2ybUfj+WKAwSNf2rHGDsqKE7P3h/Au7BvVLH+uBgYwQ42vAVDudwbNMt6uLDBzciGjYJS09Y7KVhb7gWxAPvPdLdgf4ctOXCXlnyeR+wv1iSYuQC4oRJeIMXemf5RQ0bvxK1Voez1HKuS46iCw+1DvNo8MePX/lVp46cJqkqH8SlxLWh+sg/AgQKNjhIyZfjIWbn7kOpyiuegc7/SXowRrBX2/nibkTKMbcNICR7azefJGtC9aJ3JtMULnO9TzqXIUkPZAuvM9YcItFVEp9BBl rsa-key-20190708";

        }
    }

    public static class CSharpJWTClientConfiguration
    {
        public static void Init(string issuer)
        {

            Configuration.Issuer = issuer;

            var policy = Policy.Handle<Exception>()
                                 .WaitAndRetry(5,
                                               retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                               (ex, time) =>
                                               {
                                                     //TODO
                                               });

            policy.Execute(() =>
            {
                using (var client = GetSecret())
                {
                    try
                    {
                        var response = client.Result;

                        response.EnsureSuccessStatusCode();
                        Configuration.SecurityKey = response.Content.ReadAsStringAsync().Result;
                    }
                    catch
                    {
                        throw new Exception($"Not found oauth server with host {Configuration.Issuer}");
                    }
                }
            });

        }

        private static Task<HttpResponseMessage> GetSecret()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri("http://192.168.77.100:5000");

            client.Timeout = TimeSpan.FromSeconds(5);

            return client.GetAsync("/oauth/secret.ssh");
           
        }
    }
}
