namespace CSharpJWT.Extensions
{
    using CSharpJWT.Common;
    using CSharpJWT.Common.Models;
    using CSharpJWT.Common.Services;
    using CSharpJWT.Internal;
    using CSharpJWT.Middleware;
    using CSharpJWT.Models;
    using CSharpJWT.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class CSharpJWTExtension
    {
        public static IApplicationBuilder UseCSharpJWTServer(this IApplicationBuilder app,
            Action<TokenProviderOptions> options = null)
        {
            var tokenProviderOptions = Configuration.Init(app.ApplicationServices.GetService<IConfiguration>());

            options?.Invoke(tokenProviderOptions);

            app.UseMiddleware<TokenProviderMiddleware>(tokenProviderOptions);

            //app.UseMiddleware<CSharpJWTValidateClientMiddleWare>
            return app;
        }

        public static IServiceCollection AddCSharpJWTAuthentication(this IServiceCollection services,
            Action<TokenValidationOptions> options = null)
        {
            var validationOptions = new TokenValidationOptions(Configuration.Issuer, Configuration.SecurityKey);

            options?.Invoke(validationOptions);

            services.AddSingleton(validationOptions);

            //https://medium.com/faun/asp-net-core-entity-framework-core-with-postgresql-code-first-d99b909796d7
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
