namespace CSharpJWT.Services
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using CSharpJWT.Domain;
    using CSharpJWT.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class CSharpUserManager : UserManager<User>
    {
        private readonly CSharpSignInManager _signInManager;

        private readonly ITokenService _tokenService;

        private readonly CSharpJWTContext _context;

        private const string UsernameOrPasswordIncorrect = "Username or password incorrect.";

        public CSharpUserManager(IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger,
            CSharpSignInManager signInManager,
            ITokenService tokenService,
            CSharpJWTContext context) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _signInManager = signInManager;

            _tokenService = tokenService;

            _context = context;
        }

        public static IdentityOptions CreateOptions(IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 1;
            options.User.RequireUniqueEmail = false;

            return options;
        }

        public async Task<UserResult> VerifyUserAsync(ClientResult clientResult,
            string username,
            string password,
            TokenRequest tokenRequest)
        {
            tokenRequest.User = await FindByNameAsync(username);

            tokenRequest.Client = clientResult.Client;

            if (tokenRequest.User == null) return new UserResult(new { error = UsernameOrPasswordIncorrect });

            if (!await _context.UserClients.AnyAsync(x => x.ClientId == clientResult.Client.Id && x.UserId == tokenRequest.User.Id))
                 return new UserResult(new { error = "Invalid client." });

            var result = await VerifyUserAsync(tokenRequest.User, password);

            if (!result.Succeeded) return result;

            return await GenerateBearerTokenAsync(tokenRequest);
        }

        public async Task<UserResult> VerifyUserAsync(string username,
            string password,
            TokenRequest tokenRequest)
        {
            tokenRequest.User = await FindByNameAsync(username);

            var result = await VerifyUserAsync(tokenRequest.User, password);

            if (!result.Succeeded) return result;
            
            return await GenerateBearerTokenAsync(tokenRequest);
        }

        public async Task<UserResult> RefreshAccessTokenAsync(string token, TokenRequest tokenRequest)
        {
            var refreshTokenResult = await _tokenService.VerifyRefreshTokenAsync(token);

            if (!refreshTokenResult.Succeeded) return new UserResult(refreshTokenResult.Error);

            tokenRequest.User = await FindByIdAsync(refreshTokenResult.UserId);

            tokenRequest.Client = await _context.Clients.SingleOrDefaultAsync(x => x.Id.Equals(refreshTokenResult.ClientId));

            return await GenerateBearerTokenAsync(tokenRequest);
        }

        public async Task<UserResult> GenerateBearerTokenAsync(TokenRequest tokenRequest)
        {
            var client = tokenRequest.Client;

            var user = tokenRequest.User;

            if (client != null)
            {
                tokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.ClientKeyClaimType, client.Id));

                tokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.ClientIdClaimType, client.ClientId));

                tokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.ClientNameClaimType, client.ClientName));

                tokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.AudienceClaimType, client.ClientUri));
            }
            
            tokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.IssuerClaimType, tokenRequest.Issuer));

            tokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.DefaultNameClaimType, user.Id));

            tokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.EmailClaimType, user.UserName));

            return new UserResult(await _tokenService.GenerateTokenAsync(tokenRequest));
        }

        private async Task<UserResult> VerifyUserAsync(User user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded) return new UserResult(new { error = UsernameOrPasswordIncorrect });

            return new UserResult();
        }
    }
}
