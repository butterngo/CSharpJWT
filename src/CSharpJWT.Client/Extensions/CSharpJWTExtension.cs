namespace CSharpJWT.Client.Extensions
{
    using CSharpJWT.Client.Models;
    using CSharpJWT.Client.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class CSharpJWTExtension
    {
        public static IServiceCollection AddCSharpJWTAuthentication(this IServiceCollection services,
            TokenValidationOptions tokenValidationOptions)
        {
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationOptions.GenerateTokenValidationParameters();
            });

            services.AddSingleton<ICSharpAuthenticateService, CSharpAuthenticateService>();

            services.AddHttpClient(Constant.CSharpAuthenticateService, c => c.BaseAddress = new Uri(Configuration.Issuer))
                .SetHandlerLifetime(TimeSpan.FromSeconds(5)); 
            
            return services;
        }

    }
}
