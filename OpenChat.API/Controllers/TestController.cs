using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpenChat.API.Controllers
{
    [Route("test")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("text")]
        public IActionResult GetText()
        {
            string userName = ControllerContext.HttpContext.User.Identity?.Name ?? "ERROR";
            string authType = ControllerContext.HttpContext.User.Identity?.AuthenticationType ?? "ERROR";
            return Ok($"{userName} authenticated with type {authType}");
        }
    }
}
