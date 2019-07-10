namespace CSharpJWT
{
    public static class Configuration
    {
        public static string SecurityKey { get; set; }
        public static string Audience { get; set; }
        public static string Issuer { get; set; }
        public static string PhysicalSecretPath { get; set; }
        public static bool ValidateClient { get; set; }

        public static bool ValidateAudience { get; set; }
    }
}
