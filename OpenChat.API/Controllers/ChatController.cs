using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.DTO;
using OpenChat.API.Interfaces;

namespace OpenChat.API.Controllers
{
    [Route("chats")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatManager chatManager;
        private readonly ILogoManager logoManager;

        public ChatController(IChatManager chatManager, ILogoManager logoManager)
        {
            this.chatManager = chatManager;
            this.logoManager = logoManager;
        }

        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public IActionResult CreateChat([FromForm] NewChat model)
        {
            string url = logoManager.DefaultUrl;
            if (model.Logo != null)
            {
                url = logoManager.Create(model.Logo);
            }
            chatManager.Create(model.Name, url, model.Users);
            return Ok();
        }

        [HttpPost]
        [Route("search")]
        public IActionResult Search([FromBody] string searchString)
        {
            var chats = chatManager.Chats.Where(c => c.Name.Contains(searchString))
                .Select(c => new ChatPreview() { Id = c.Id, LogoUrl = c.LogoUrl, Name = c.Name, LastMessage = "Пока пусто" });
            return Ok(chats);
        }
    }
}
