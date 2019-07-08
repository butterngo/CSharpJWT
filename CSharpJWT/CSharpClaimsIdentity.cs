namespace CSharpJWT
{
    using System.Security.Claims;

    public class CSharpClaimsIdentity : ClaimsIdentity
    {
        public const string EmailClaimType = "email";
        public const string AudienceClaimType = "au";
        public const string IssuerClaimType = "iss";
        public const string ClientKeyClaimType = "clKey";
        public const string ClientIdClaimType = "clId";
    }
}
