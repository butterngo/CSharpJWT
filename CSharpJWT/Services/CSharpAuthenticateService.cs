namespace CSharpJWT.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using CSharpJWT.Caches;
    using CSharpJWT.Extensions;
    using Microsoft.AspNetCore.Http;

    public class CSharpAuthenticateService: ICSharpAuthenticateService
    {
        private readonly HttpClient _client;

        private const string CachePath = "/oauth/cache";

        public CSharpAuthenticateService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(Configuration.Issuer);
            _client.Timeout = TimeSpan.FromSeconds(3);
        }

        public async Task<bool> ValidateTokenAsync(HttpContext context)
        {
            try
            {
                var cacheKey = context.User.Claims.GetValue(CSharpClaimsIdentity.CacheKeyClaimType);

                var headers = context.Request.Headers["Authorization"];

                var token = headers.ToString().Split(' ')[1];

                var response =  await _client.GetAsync($"{CachePath}?key={cacheKey}");

                response.EnsureSuccessStatusCode();

                var tokenInformation = response.Content.ReadAsStringAsync().Result.ToObj<TokenInformation>();

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

        public bool ValidateToken(HttpContext context)
        {
            return ValidateTokenAsync(context).Result;
        }

        public bool ValidateAudience(HttpContext context, List<string> audiences)
        {
            if (audiences.Count() == 0) return true;

            try
            {
                audiences.Add(CurrentAudience(context));

                var array = context.User.Claims.GetValue(CSharpClaimsIdentity.AudienceClaimType).Split(',');

                return array.Where(x => audiences.Contains(x)).Any();
            }
            catch
            {
                return false;
            }
        }

        public bool ValidateRole(HttpContext context, IEnumerable<string> roles)
        {
            if (roles.Count() == 0) return true;

            try
            {
                var array = context.User.Claims.GetValue(CSharpClaimsIdentity.DefaultRoleClaimType).Split(',');

                return array.Where(x => roles.Contains(x)).Any();
            }
            catch
            {
                return false;
            }
            
        }

        public bool ValidateIssuer(HttpContext context)
        {
            var issuer = context.User.Claims.GetValue(CSharpClaimsIdentity.IssuerClaimType);

            return Configuration.Issuer.Equals(issuer);
        }

        private string CurrentAudience(HttpContext context)
        {
            var request = context.Request;

            return $"{request.Scheme}://{request.Host.ToString()}";
        }
    }
}
