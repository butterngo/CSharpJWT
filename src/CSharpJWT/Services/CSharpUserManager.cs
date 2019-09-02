namespace CSharpJWT.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using CSharpJWT.Common;
    using CSharpJWT.Domain;
    using CSharpJWT.Internal.Services;
    using CSharpJWT.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class CSharpUserManager : UserManager<User>
    {
        private readonly CSharpSignInManager _signInManager;

        private readonly ITokenService _tokenService;

        private readonly IClaimService _claimService;

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
            IClaimService claimService,
            CSharpJWTContext context) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _signInManager = signInManager;

            _tokenService = tokenService;

            _claimService = claimService;

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
            tokenRequest.User = await _context.Users.SingleOrDefaultAsync(x=>x.UserName.Equals(username));

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
            tokenRequest.User = await _context.Users.SingleOrDefaultAsync(x => x.UserName.Equals(username));

            var result = VerifyUser(tokenRequest.User, password);

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
            tokenRequest = await _claimService.GenerateClaimAsync(new ClaimRequest(tokenRequest));

            return new UserResult(await _tokenService.GenerateTokenAsync(tokenRequest));
        }

        private async Task<UserResult> VerifyUserAsync(User user, string password)
        {
            if (user == null) return new UserResult(new { error = UsernameOrPasswordIncorrect });

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded) return new UserResult(new { error = UsernameOrPasswordIncorrect });

            return new UserResult();
        }

        private UserResult VerifyUser(User user, string password)
        {
            if(user == null) return new UserResult(new { error = UsernameOrPasswordIncorrect });

            var result = PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            
            if (result == PasswordVerificationResult.Failed) return new UserResult(new { error = UsernameOrPasswordIncorrect });

            return new UserResult();
        }
    }
}
