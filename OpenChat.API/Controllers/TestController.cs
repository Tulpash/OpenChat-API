using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.Interfaces;
using OpenChat.API.Managers;

namespace OpenChat.API.Controllers
{
    [Route("test")]
    [ApiController]
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        private readonly IConnectionManager connectionManager;

        public TestController(IConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        [HttpGet]
        [Route("text")]
        public IActionResult GetText()
        {
            string userName = ControllerContext.HttpContext.User.Identity?.Name ?? "ERROR";
            string authType = ControllerContext.HttpContext.User.Identity?.AuthenticationType ?? "ERROR";
            return Ok($"{userName} authenticated with type {authType}");
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetChat()
        {           
            return Ok(connectionManager.Users);
        }
    }
}
