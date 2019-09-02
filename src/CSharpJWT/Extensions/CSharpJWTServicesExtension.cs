namespace CSharpJWT.Extensions
{
    using CSharpJWT.Internal.Services;
    using CSharpJWT.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class CSharpJWTServicesExtension
    {
        public static IServiceCollection AddCSharpUserManager(this IServiceCollection services)
        {
            services.AddScoped<CSharpUserManager>();

            return services;
        }

        public static IServiceCollection AddCSharpSignInManager(this IServiceCollection services)
        {
            services.AddScoped<CSharpSignInManager>();

            return services;
        }

        public static IServiceCollection AddClientService(this IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();

            return services;
        }

        public static IServiceCollection AddClaimService(this IServiceCollection services)
        {
            services.AddScoped<IClaimService, ClaimService>();

            return services;
        }
    }
}
