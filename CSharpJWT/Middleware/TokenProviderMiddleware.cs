﻿namespace CSharpJWT.Middleware
{
    using CSharpJWT.Models;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    public class TokenProviderMiddleware
    {
        public readonly RequestDelegate _next;

        public readonly TokenProviderOptions _options;

        private const string SecretPath = "/oauth/secret.ssh";

        private const string CachePath = "/oauth/cache";

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
            else if (context.Request.Path.Equals(SecretPath, StringComparison.Ordinal))
            {
                await DownloadSecretKeyAsync(context);
            }
            else if (context.Request.Path.Equals(CachePath, StringComparison.Ordinal))
            {
                await new CacheHandler().ExecuteAsync(context);
            }
            else
            {
                await _next(context);
            }
        }

        private async Task DownloadSecretKeyAsync(HttpContext context)
        {
            string secretKey = Configuration.SecurityKey;

            //if (System.IO.File.Exists(Configuration.PhysicalSecretPath))
            //{
            //    secretKey = System.IO.File.ReadAllText(Configuration.PhysicalSecretPath);
            //}

            context.Response.StatusCode = 200;

            context.Response.ContentType = "text/plain";

            await context.Response.WriteAsync(secretKey);
        }
    }
}