namespace CSharpJWT.Common
{
    using System.Security.Claims;

    public class CSharpClaimsIdentity : ClaimsIdentity
    {
        public const string UserNameClaimType = "username";
        public const string AudienceClaimType = "au";
        public const string IssuerClaimType = "iss";
        public const string ClientKeyClaimType = "clKey";
        public const string ClientIdClaimType = "clId";
        public const string ClientNameClaimType = "clName";
        public const string CacheKeyClaimType = "cacheKey";
        public const string RefreshTokenClaimType = "refresh_token";
        public const string ScopesClaimType = "scopes";
    }
}
