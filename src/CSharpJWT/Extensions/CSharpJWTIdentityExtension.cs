namespace CSharpJWT.Extensions
{
    using CSharpJWT.Domain;
    using CSharpJWT.Internal.Services;
    using CSharpJWT.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class CSharpJWTIdentityExtension
    {
        public static IServiceCollection AddCSharpJWTIdentity<TContext>(this IServiceCollection services)
          where TContext : CSharpJWTContext
        {

            services.AddIdentity<User, Role>(Options => CSharpUserManager.CreateOptions(Options))
                .AddEntityFrameworkStores<TContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<ITokenService, TokenService>();

            return services
                .AddCSharpUserManager()
                .AddCSharpSignInManager()
                .AddClientService();
        }
    }
}
