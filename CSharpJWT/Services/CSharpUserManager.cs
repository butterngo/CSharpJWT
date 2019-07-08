namespace CSharpJWT.Services
{
    using System;
    using System.Collections.Generic;
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

        public async Task<UserResult> VerifyUserAsync(string clientId,
            string username,
            string password,
            TokenRequest tokenRequest)
        {
            var user = await FindByNameAsync(username);

            if(user == null) return new UserResult(new { error = UsernameOrPasswordIncorrect });

            if (!await _context.UserClients.AnyAsync(x => x.ClientId == clientId && x.UserId == user.Id))
                 return new UserResult(new { error = "Invalid client." });
            
            return await VerifyUserAsync(username, password, tokenRequest);
        }

        public async Task<UserResult> VerifyUserAsync(string username,
            string password,
            TokenRequest tokenRequest)
        {
            var user = await FindByNameAsync(username);

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded) return new UserResult(new { error = UsernameOrPasswordIncorrect });

            return await GenerateBearerTokenAsync(user, tokenRequest);
        }

        public async Task<UserResult> RefreshAccessTokenAsync(string token, TokenRequest tokenRequest)
        {
            var refreshTokenResult = await _tokenService.VerifyRefreshTokenAsync(token);

            if (!refreshTokenResult.Successed) return new UserResult(refreshTokenResult.Error);

            var user = await FindByIdAsync(refreshTokenResult.UserId);

            return await GenerateBearerTokenAsync(user, tokenRequest);
        }

        public async Task<UserResult> GenerateBearerTokenAsync(User user, TokenRequest tokenRequest)
        {
            tokenRequest.Claims.Add(new CustomClaim(CSharpClaimsIdentity.IssuerClaimType, tokenRequest.Issuer));

            tokenRequest.Claims.Add(new CustomClaim(CSharpClaimsIdentity.DefaultNameClaimType, user.Id));

            tokenRequest.Claims.Add(new CustomClaim(CSharpClaimsIdentity.EmailClaimType, user.UserName));

            return new UserResult(await _tokenService.GenerateTokenAsync(tokenRequest));
        }

    }
}
