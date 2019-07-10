namespace CSharpJWT.Services
{
    using CSharpJWT.Caches;
    using CSharpJWT.Domain;
    using CSharpJWT.Extensions;
    using CSharpJWT.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
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
        private readonly JWTTokenDistributedCache _cache;

        private readonly CSharpJWTContext _context;

        public TokenService(JWTTokenDistributedCache cache, CSharpJWTContext context)
        {
            _cache = cache;

            _context = context;
        }

        public async Task<IDictionary<string, object>> GenerateTokenAsync(TokenRequest dto)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(dto.SecurityKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            string cacheKey = $"access_token_{Guid.NewGuid().ToString()}";

            string refreshToken = await GenerateRefreshTokenAsync(dto);

            dto.Claims.Add(new Claim(CSharpClaimsIdentity.CacheKeyClaimType, cacheKey));

            dto.Claims.Add(new Claim(CSharpClaimsIdentity.RefreshTokenClaimType, refreshToken));

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: dto.Issuer,
                audience: dto.Audience,
                claims: dto.Claims,
                expires: DateTime.UtcNow.Add(dto.TokenExpiration),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            dto.Responses.Add("access_token", token);

            dto.Responses.Add("token_expiration", dto.TokenExpiration.TotalSeconds);

            dto.Responses.Add("refresh_token", refreshToken);

            dto.Responses.Add("refresh_token_expiration", dto.RefreshTokenExpiration.TotalSeconds);

            var keys = dto.Responses.Keys.OrderBy(x => x);

            var dic = new Dictionary<string, object>();

            foreach (string key1 in keys)
            {
                dic.Add(key1, dto.Responses[key1]);
            }

            dic.Add("token_type", "Bearer");

            await _cache.SetAsync(cacheKey, dto, token);

            return dic;
        }

        public async Task<RefreshTokenResult> VerifyRefreshTokenAsync(string token)
        {
            var refreshToken = await _context
           .RefreshTokens
           .Include(x=>x.User)
           .Include(x=>x.Client)
           .SingleOrDefaultAsync(x => x.Id == token);

            if (refreshToken == null) return new RefreshTokenResult(new { error = "token invalid." });

            if (refreshToken.ExpirationDate <= DateTime.UtcNow) return new RefreshTokenResult(new { error = "token expired." });

            _context.RefreshTokens.Remove(refreshToken);

            await _context.SaveChangesAsync();

            return new RefreshTokenResult(refreshToken.ClientId, refreshToken.UserId);
        }

        public async Task<bool> IsValidTokenAsync(HttpContext context)
        {
            try
            {
                var cacheKey = context.User.Claims.GetValue(CSharpClaimsIdentity.CacheKeyClaimType);

                var headers = context.Request.Headers["Authorization"];

                var token = headers.ToString().Split(' ')[1];

                var tokenInformation = await _cache.GetAsync(cacheKey);

                if (tokenInformation == null) return false;

                if (!tokenInformation.Token.Equals(token)) return false;

                if (tokenInformation.ExpirationDate <= DateTime.UtcNow) return false;

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool IsValidToken(HttpContext context)
        {
            return IsValidTokenAsync(context).Result;
        }

        public async Task<bool> RevokeTokenAsync(AuthenticateResult authenticateResult)
        {
            var cacheKey = authenticateResult.Principal.Claims.GetValue(CSharpClaimsIdentity.CacheKeyClaimType);

            var refreshTokenKey = authenticateResult.Principal.Claims.GetValue(CSharpClaimsIdentity.RefreshTokenClaimType);

            var refreshToken = await _context
              .RefreshTokens.SingleOrDefaultAsync(x => x.Id.Equals(refreshTokenKey));

            if (refreshToken == null) return false;

            _context.RefreshTokens.Remove(refreshToken);

            await _cache.RemoveAsync(cacheKey);

            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<string> GenerateRefreshTokenAsync(TokenRequest dto)
        {
            var clientKey = dto.Claims.GetValue(CSharpClaimsIdentity.ClientKeyClaimType);

            var userId = dto.Claims.GetValue(CSharpClaimsIdentity.DefaultNameClaimType);

            var expirationDate = DateTime.UtcNow.Add(dto.RefreshTokenExpiration);

            var guid = GenerateGuid(expirationDate).ToString("n");

            string token = $"{clientKey}.{userId}.{guid}";

            _context.RefreshTokens.Add(new RefreshToken
            {
                Id = token,
                ClientId = string.IsNullOrEmpty(clientKey) ? null : clientKey,
                UserId = userId,
                ExpirationDate = expirationDate,
            });

            await _context.SaveChangesAsync();

            return token;

        }

        private Guid GenerateGuid(DateTime time)
        {
            var tempGuid = Guid.NewGuid();
            var bytes = tempGuid.ToByteArray();
            bytes[3] = (byte)time.Year;
            bytes[2] = (byte)time.Month;
            bytes[1] = (byte)time.Day;
            bytes[0] = (byte)time.Hour;
            bytes[5] = (byte)time.Minute;
            bytes[4] = (byte)time.Second;
            return new Guid(bytes);
        }
    }
}
