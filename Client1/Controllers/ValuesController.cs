namespace Client1.Controllers
{
    using System.Collections.Generic;
    using CSharpJWT;
    using CSharpJWT.Attributes;
    using CSharpJWT.Extensions;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [CSharpAuthorization(
          Audiences: new string[] { "https://c-sharp.vn" }
        , Roles: new string[] { "Administrator", "User" })]
        public ActionResult<object> Get()
        {
            return new 
            {
                ClientId = User.Claims.GetValue(CSharpClaimsIdentity.ClientIdClaimType),
                Audience = User.Claims.GetValue(CSharpClaimsIdentity.AudienceClaimType),
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
