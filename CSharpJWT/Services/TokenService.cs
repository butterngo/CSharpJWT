namespace CSharpJWT.Services
{
    using CSharpJWT.Domain;
    using CSharpJWT.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class TokenService : ITokenService
    {
        private readonly IDistributedCache _cache;

        private readonly CSharpJWTContext _context;

        private const string StringCipherPassword = "csharpvn";

        public TokenService(IDistributedCache cache, CSharpJWTContext context)
        {
            _cache = cache;

            _context = context;
        }

        public async Task<IDictionary<string, object>> GenerateTokenAsync(TokenRequest dto)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(dto.SecurityKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();

            foreach (var claim in dto.Claims)
            {
                claims.Add(new Claim(claim.Type, claim.Value.ToString()));
            }

            var token = new JwtSecurityToken(
                issuer: dto.Issuer,
                audience: dto.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(dto.TokenExpiration),
                signingCredentials: creds);

            dto.Responses.Add("access_token", new JwtSecurityTokenHandler().WriteToken(token));

            dto.Responses.Add("token_expiration", dto.TokenExpiration.TotalSeconds);

            dto.Responses.Add("refresh_token", await GenerateRefreshTokenAsync(dto));

            dto.Responses.Add("refresh_token_expiration", dto.RefreshTokenExpiration.TotalSeconds);

            var keys = dto.Responses.Keys.OrderBy(x => x);

            var dic = new Dictionary<string, object>();

            foreach (string key1 in keys)
            {
                dic.Add(key1, dto.Responses[key1]);
            }

            dic.Add("token_type", "Bearer");

            return dic;
        }

        public async Task<RefreshTokenResult> VerifyRefreshTokenAsync(string token)
        {
            var refreshToken = await _context
           .RefreshTokens
           .SingleOrDefaultAsync(x => x.Id == token);

            if (refreshToken == null) return new RefreshTokenResult(new { error = "token invalid." });

            if(refreshToken.ExpirationDate <= DateTime.UtcNow) return new RefreshTokenResult(new { error = "token expired." });

            return StringCipher.DecryptString(token, StringCipherPassword).ToObj<RefreshTokenResult>();
        }

        private async Task<string> GenerateRefreshTokenAsync(TokenRequest dto)
        {
            var clientKey = dto.Claims.FirstOrDefault(x => x.Type.Equals(CSharpClaimsIdentity.ClientKeyClaimType))?.Value.ToString();

            var userId = dto.Claims.FirstOrDefault(x => x.Type.Equals(CSharpClaimsIdentity.DefaultNameClaimType))?.Value.ToString();

            var expirationDate = DateTime.UtcNow.Add(dto.RefreshTokenExpiration);

            var json = new RefreshTokenResult(clientKey, userId, expirationDate).ToJson();

            string token = StringCipher.EncryptString(json, StringCipherPassword);

            var refreshToken = await _context
                .RefreshTokens
                .SingleOrDefaultAsync(x => x.ClientId == clientKey && x.UserId == userId);

            if (refreshToken != null) _context.RefreshTokens.Remove(refreshToken);

            _context.RefreshTokens.Add(new RefreshToken
            {
                Id = token,
                ClientId = clientKey,
                UserId = userId,
                ExpirationDate = expirationDate,
            });

            await _context.SaveChangesAsync();

            return token;

        }
    }
}
