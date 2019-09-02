namespace CSharpJWT.Services
{
    using CSharpJWT.Common;
    using CSharpJWT.Domain;
    using CSharpJWT.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class ClaimService: IClaimService
    {
        private readonly CSharpJWTContext _context;

        public ClaimService(CSharpJWTContext context)
        {
            _context = context;
        }

        public async Task<TokenRequest> GenerateClaimAsync(ClaimRequest claimRequest)
        {
            var client = claimRequest.TokenRequest.Client;

            var user = claimRequest.TokenRequest.User;

            var roles = await GetRoleNameAsync(user);

            var scopes = await GetScopeNameAsync(client);

            if (client != null)
            {
                claimRequest.TokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.ClientKeyClaimType, client.Id));

                claimRequest.TokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.ClientIdClaimType, client.ClientId));

                claimRequest.TokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.ClientNameClaimType, client.ClientName));

                claimRequest.TokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.AudienceClaimType, client.ClientUri));

                claimRequest.TokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.ScopesClaimType,
                    string.Join(",", scopes)));

            }

            claimRequest.TokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.IssuerClaimType, claimRequest.TokenRequest.Issuer));

            claimRequest.TokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.DefaultNameClaimType, user.Id));

            claimRequest.TokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.DefaultRoleClaimType,
                string.Join(",", roles)));

            claimRequest.TokenRequest.Claims.Add(new Claim(CSharpClaimsIdentity.UserNameClaimType, user.UserName));

            return claimRequest.TokenRequest;
        }

        private async Task<IList<string>> GetRoleNameAsync(User user)
        {
            return await _context.UserRoles.Join(_context.Roles,
                userRole => userRole.RoleId, role => role.Id,
                (userRole, role) => new { userRole, role })
                .Where(x => x.userRole.UserId.Equals(user.Id))
                .Select(x => x.role.Name)
                .ToListAsync();
        }

        private async Task<IList<string>> GetScopeNameAsync(Client client)
        {
            return await _context.ClientScopes.Join(_context.Scopes,
                clientScope => clientScope.ScopeId, scope => scope.Id,
                (clientScope, scope) => new { clientScope, scope })
                .Where(x => x.clientScope.ClientId.Equals(client.Id))
                .Select(x => x.scope.Name)
                .ToListAsync();
        }
    }
}
