namespace CSharpJWT.Middleware
{
    using CSharpJWT.Models;
    using CSharpJWT.Services;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
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
            // If the request path doesn't match, skip
            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                await _next(context);
                return;
            }

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST")
               || !context.Request.HasFormContentType)
            {

                await BadRequest(context, new { error = "Bad request.." });

                return;
            }

            GrantTypeEnum grantType = GrantTypeEnum.password;

            try
            {
                grantType = (GrantTypeEnum)Enum.Parse(typeof(GrantTypeEnum), context.Request.Form["grant_type"], true);
            }
            catch
            {
                await BadRequest(context, new { error = "invail grant_type" });

                return;
            }


            string clientId = string.Empty;

            if (_options.VerifyClient)
            {
                var headers = context.Request.Headers;

                string secretKey = string.Empty;

                try
                {
                    if (!string.IsNullOrEmpty(headers["Authorization"]))
                    {
                        var array = headers["Authorization"].ToString().Split(' ');
                        secretKey = array[1];
                    }
                }
                catch
                {
                    await BadRequest(context, new { error = "Invalid client" });

                    return;
                }

                IServiceProvider serviceProvider = context.RequestServices;

                var clientService = (IClientService)serviceProvider.GetService(typeof(IClientService));

                clientId = await clientService.VerifyClientAsync(secretKey);

                if (string.IsNullOrEmpty(clientId))
                {
                    await BadRequest(context, new { error = "Invalid client" });

                    return;
                }

            }

            switch (grantType)
            {
                case GrantTypeEnum.password:
                    await GenerateTokenByUserNamePassword(context, clientId,
                        context.Request.Form["username"],
                        context.Request.Form["password"]);
                    break;
                case GrantTypeEnum.refresh_token:
                    await GenerateTokenByRefreshToken(context, clientId,
                        context.Request.Form["refresh_token"]);
                    break;
            }

        }

        private async Task GenerateTokenByUserNamePassword(HttpContext context,
            string clientId,
            string username,
            string password)
        {
            
            var tokenRequest = new TokenRequest(_options);

            IServiceProvider serviceProvider = context.RequestServices;

            var userManager = (CSharpUserManager)serviceProvider.GetService(typeof(CSharpUserManager));

            var userResult = new UserResult();

            if (!string.IsNullOrEmpty(clientId))
            {
                userResult = await userManager.VerifyUserAsync(clientId, username, password, tokenRequest);
            }
            else
            {
                userResult = await userManager.VerifyUserAsync(username, password, tokenRequest);
            }

            await ResponseAsync(context, userResult);

        }

        private async Task GenerateTokenByRefreshToken(HttpContext context, string clientId, string refreshToken)
        {
            try
            {
                IServiceProvider serviceProvider = context.RequestServices;

                var userManager = (CSharpUserManager)serviceProvider.GetService(typeof(CSharpUserManager));

                var tokenRequest = new TokenRequest(_options);

                var userResult = await userManager.RefreshAccessTokenAsync(refreshToken, tokenRequest);

                await ResponseAsync(context, userResult);

            }
            catch (Exception ex)
            {
                await BadRequest(context, new { error = ex.Message });

                return;
            }

        }

        private async Task BadRequest(HttpContext context, object msg)
        {
            context.Response.StatusCode = 400;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(msg.ToJson());

            return;
        }

        private async Task ResponseAsync(HttpContext context, UserResult userResult)
        {
            if (userResult.Successed)
            {
                context.Response.StatusCode = 200;

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(userResult.Token.ToJson());

                return;
            }
            else
            {
                context.Response.StatusCode = 400;

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(userResult.Error.ToJson());

                return;
            }
        }
    }
}
