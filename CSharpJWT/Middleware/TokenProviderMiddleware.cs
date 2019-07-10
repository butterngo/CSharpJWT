namespace CSharpJWT.Middleware
{
    using CSharpJWT.Models;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    public class TokenProviderMiddleware
    {
        public readonly RequestDelegate _next;

        public readonly TokenProviderOptions _options;

        public TokenProviderMiddleware(RequestDelegate next,
            TokenProviderOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Equals(_options.TokenPath, StringComparison.Ordinal))
            {
                await new TokenHandler(_options).ExecuteAsync(context);
            }
            else if (context.Request.Path.Equals(_options.RevokePath, StringComparison.Ordinal))
            {
                await new RevokeTokenHandle().ExecuteAsync(context);
            }
            else
            {
                await _next(context);
            }
        }
    }
}
