namespace CSharpJWT.Services
{
    using CSharpJWT.Models;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public interface IClaimService
    {
        Task<TokenRequest> GenerateClaimAsync(ClaimRequest claimRequest);
    }
}
