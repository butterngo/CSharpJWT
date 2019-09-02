namespace CSharpJWT.Models
{
    public class ClaimRequest
    {
        public ClaimRequest(TokenRequest tokenRequest)
        {
            TokenRequest = tokenRequest;
        }

        public TokenRequest TokenRequest { get; set; }
    }
}
