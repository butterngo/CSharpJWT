namespace CSharpJWT.Internal.Caches
{
    using CSharpJWT.Extensions;
    using CSharpJWT.Internal;
    using CSharpJWT.Models;
    using Microsoft.Extensions.Caching.Distributed;
    using System;
    using System.Threading.Tasks;

    internal class JWTTokenDistributedCache
    {
        private readonly IDistributedCache _cache;

        public JWTTokenDistributedCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetAsync(string key, TokenRequest dto, string token)
        {
            MapData(dto, token, out byte[] bytes, out DistributedCacheEntryOptions options);

            await _cache.SetAsync(key, bytes, options);   
        }

        public void set(string key, TokenRequest dto, string token)
        {
             MapData(dto, token, out byte[] bytes, out DistributedCacheEntryOptions options);

            _cache.Set(key, bytes, options);
        }

        public async Task<TokenInformation> GetAsync(string key)
        {
            var bytes = await _cache.GetAsync(key);

            if (bytes == null) return null;

            return bytes.ToObj<TokenInformation>();
        }

        public TokenInformation Get(string key)
        {
            var bytes = _cache.Get(key);

            if (bytes == null) return null;

            return bytes.ToObj<TokenInformation>();
        }

        public Task RemoveAsync(string key) => _cache.RemoveAsync(key);

        private void MapData(TokenRequest dto, string token, out byte[] bytes,
            out DistributedCacheEntryOptions options)
        {
            var clientKey = dto.Claims.GetValue(CSharpClaimsIdentity.ClientKeyClaimType);

            var userId = dto.Claims.GetValue(CSharpClaimsIdentity.DefaultNameClaimType);

            DateTime expirationDate = DateTime.UtcNow.Add(dto.TokenExpiration);

            bytes = new TokenInformation(clientKey, userId, token, expirationDate).ToBytes();

            options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expirationDate,
                SlidingExpiration = dto.TokenExpiration
            };

        }
    }

}
