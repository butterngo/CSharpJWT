namespace CSharpJWT.Middleware
{
    using CSharpJWT.Extensions;
    using CSharpJWT.Models;
    using CSharpJWT.Services;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    public class TokenHandler
    {
        private readonly TokenProviderOptions _options;

        public TokenHandler(TokenProviderOptions options)
        {
            _options = options;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST")
             || !context.Request.HasFormContentType)
            {

                await context.BadRequestAsync(new { error = "Bad request.." });

                return;
            }

            GrantTypeEnum grantType = GrantTypeEnum.password;

            try
            {
                grantType = (GrantTypeEnum)Enum.Parse(typeof(GrantTypeEnum), context.Request.Form["grant_type"], true);
            }
            catch
            {
                await context.BadRequestAsync(new { error = "invail grant_type" });

                return;
            }

            ClientResult clientResult = null;

            if (_options.ValidateClient && grantType != GrantTypeEnum.refresh_token)
            {
                clientResult = await VerifyClientAsync(context);

                if (!clientResult.Succeeded)
                {
                    await context.BadRequestAsync(clientResult.Error);

                    return;
                }
            }

            switch (grantType)
            {
                case GrantTypeEnum.password:
                    await GenerateTokenByUserNamePassword(context, clientResult,
                        context.Request.Form["username"],
                        context.Request.Form["password"]);
                    break;
                case GrantTypeEnum.refresh_token:
                    await GenerateTokenByRefreshToken(context, context.Request.Form["refresh_token"]);
                    break;
            }
        }

        private async Task<ClientResult> VerifyClientAsync(HttpContext context)
        {
            IServiceProvider serviceProvider = context.RequestServices;

            var clientService = (IClientService)serviceProvider.GetService(typeof(IClientService));

            return await clientService.VerifyClientAsync(context);
        }

        private async Task GenerateTokenByUserNamePassword(HttpContext context,
            ClientResult clientResult,
            string username,
            string password)
        {

            var tokenRequest = new TokenRequest(_options);

            IServiceProvider serviceProvider = context.RequestServices;

            var userManager = (CSharpUserManager)serviceProvider.GetService(typeof(CSharpUserManager));

            var userResult = new UserResult();

            if (clientResult != null)
            {
                userResult = await userManager.VerifyUserAsync(clientResult, username, password, tokenRequest);
            }
            else
            {
                userResult = await userManager.VerifyUserAsync(username, password, tokenRequest);
            }

            await ResponseAsync(context, userResult);

        }

        private async Task GenerateTokenByRefreshToken(HttpContext context, string refreshToken)
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
                await context.BadRequestAsync(new { error = ex.Message });

                return;
            }

        }

        private async Task ResponseAsync(HttpContext context, UserResult userResult)
        {
            if (userResult.Succeeded)
            {
                await context.OkAsync(userResult.Token);
            }
            else
            {
                await context.BadRequestAsync(userResult.Error);
            }
        }
    }
}
