namespace CSharpJWT.Client.Extensions
{
    using CSharpJWT.Client.Services;
    using CSharpJWT.Common;
    using CSharpJWT.Common.Models;
    using CSharpJWT.Common.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class CSharpJWTExtension
    {
        public static IServiceCollection AddCSharpJWTAuthentication(this IServiceCollection services,
             Action<TokenValidationOptions> options = null)
        {
            var validationOptions = new TokenValidationOptions(Configuration.Issuer, Configuration.SecurityKey);

            options?.Invoke(validationOptions);

            services.AddSingleton(validationOptions);

            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = validationOptions.GenerateTokenValidationParameters();
            });

            services.AddSingleton<ICSharpAuthenticateService, CSharpAuthenticateService>();

            services.AddHttpClient(Constant.CSharpAuthenticateService, c => c.BaseAddress = new Uri(Configuration.Issuer))
                .SetHandlerLifetime(TimeSpan.FromSeconds(5)); 
            
            return services;
        }

    }
}
