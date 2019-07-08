namespace OAuthServer.Controllers
{
    using CSharpJWT.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("{clientId}/{secret}")]
        public async Task<ActionResult<string>> GenerateSecretByClientId(string clientId)
        {
            return await _clientService.GetSecretKeyByClientIdAsync(clientId);
        }
    }
}
