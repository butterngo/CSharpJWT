namespace CSharpJWT.Extensions
{
    using CSharpJWT.Internal;
    using CSharpJWT.Middleware;
    using CSharpJWT.Models;
    using CSharpJWT.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Text;

    public static class CSharpJWTExtension
    {
        public static IApplicationBuilder UseCSharpJWTServer(this IApplicationBuilder app,
            Action<TokenProviderOptions> options = null)
        {
            var tokenProviderOptions = Configuration.Init(app.ApplicationServices.GetService<IConfiguration>());

            options?.Invoke(tokenProviderOptions);

            app.UseMiddleware<TokenProviderMiddleware>(tokenProviderOptions);

            return app;
        }

        public static IServiceCollection AddCSharpJWTAuthentication(this IServiceCollection services)
        {
            //https://medium.com/faun/asp-net-core-entity-framework-core-with-postgresql-code-first-d99b909796d7
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

            services.AddSingleton<ICSharpAuthenticateService, CSharpAuthenticateService>();

            services.AddHttpClient(Constant.CSharpAuthenticateService, c => c.BaseAddress = new Uri(Configuration.Issuer))
             .SetHandlerLifetime(TimeSpan.FromSeconds(5));

            return services;
        }

       
    }

    //public static class CSharpJWTClientConfiguration
    //{
    //    public static void Init(string issuer)
    //    {
    //        Configuration.Issuer = issuer;

    //        using (var client = new HttpClient())
    //        {
    //            try
    //            {
    //                client.BaseAddress = new Uri(issuer);

    //                client.Timeout = TimeSpan.FromSeconds(5);

    //                var response = client.GetAsync("/oauth/secret.ssh").Result;

    //                response.EnsureSuccessStatusCode();

    //                Configuration.SecurityKey = response.Content.ReadAsStringAsync().Result;
    //            }
    //            catch
    //            {
    //                throw new Exception($"Not found oauth server with host {Configuration.Issuer}");
    //            }
    //        }

    //    }
    //}
}
