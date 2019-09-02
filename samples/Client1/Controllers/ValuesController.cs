namespace Client1.Controllers
{
    using CSharpJWT.Authentication;
    using CSharpJWT.Common;
    using CSharpJWT.Common.Utilities;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        //[CSharpAuthorize(
        //  Audiences: new string[] { "https://c-sharp.vn" }
        //, Roles: new string[] { "Administrator", "User" })]
        [CSharpAuthorize]
        public ActionResult<object> Get()
        {
            return new 
            {
                ClientId = User.Claims.GetValue(CSharpClaimsIdentity.ClientIdClaimType),
                UserId = User.Claims.GetValue(CSharpClaimsIdentity.DefaultNameClaimType),
                UserName = User.Claims.GetValue(CSharpClaimsIdentity.UserNameClaimType),
                Role = User.Claims.GetValue(CSharpClaimsIdentity.DefaultRoleClaimType),
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
