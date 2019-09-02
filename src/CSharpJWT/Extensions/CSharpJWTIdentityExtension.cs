namespace CSharpJWT.Extensions
{
    using CSharpJWT.Domain;
    using CSharpJWT.Internal.Services;
    using CSharpJWT.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class CSharpJWTIdentityExtension
    {
        public static IServiceCollection AddCSharpJWTIdentity<TContext>(this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
          where TContext : CSharpJWTContext
        {
            services.AddDbContext<TContext>(optionsAction);

            services.AddIdentity<User, Role>(Options => CSharpUserManager.CreateOptions(Options))
                .AddEntityFrameworkStores<TContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<ITokenService, TokenService>();

            return services
                .AddCSharpUserManager()
                .AddCSharpSignInManager()
                .AddClientService()
                .AddClaimService();
        }
    }
}
