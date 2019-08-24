namespace CSharpJWT.Extensions
{
    using CSharpJWT.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public static class ClaimsExtension
    {
        public static string GetValue(this IEnumerable<Claim> claims, string type)
        {
            var claim = claims.SingleOrDefault(x => x.Type.Equals(type));

            if (claim == null) return string.Empty;

            return claim.Value;
        }

        public static bool IsExistsType(this IEnumerable<Claim> claims, string type)
        {
            return claims.Any(x => x.Type.Equals(type));
        }

        public static bool IsValidValue(this IEnumerable<Claim> claims, string type, string value)
        {
            var claim = claims.SingleOrDefault(x => x.Type.Equals(type));

            if (claim == null) return false;

            return claim.Value == value;
        }
    }
}
