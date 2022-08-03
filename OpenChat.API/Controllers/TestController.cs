using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.Managers;

namespace OpenChat.API.Controllers
{
    [Route("test")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly ChatManager chatManager;

        public TestController(ChatManager chatManager)
        {
            this.chatManager = chatManager;
        }

        [HttpGet]
        [Route("text")]
        public IActionResult GetText()
        {
            string userName = ControllerContext.HttpContext.User.Identity?.Name ?? "ERROR";
            string authType = ControllerContext.HttpContext.User.Identity?.AuthenticationType ?? "ERROR";
            return Ok($"{userName} authenticated with type {authType}");
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddToChat()
        {
            chatManager.AddConnection(ControllerContext.HttpContext.User.Identity?.Name, "wugbeig");
            return Ok();
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetChat()
        {           
            return Ok(chatManager.Users);
        }
    }
}
