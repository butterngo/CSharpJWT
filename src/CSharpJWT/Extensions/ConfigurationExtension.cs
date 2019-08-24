namespace CSharpJWT.Extensions
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtension
    {
        public static T TryGetValue<T>(this IConfiguration configuration, string key)
        {
            try
            {
                return configuration.GetValue<T>(key);
            }
            catch
            {
                return default(T);
            }
        }

        public static string TrySecurityKey(this IConfiguration configuration, string key)
        {
            string value = configuration.TryGetValue<string>(key);

            if (string.IsNullOrEmpty(value)) return "security-key-csharp-vn";

            return value;
        }
    }
}
