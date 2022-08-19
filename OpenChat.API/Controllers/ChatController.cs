using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenChat.API.DTO;
using OpenChat.API.Interfaces;
using OpenChat.API.Models;

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
            chatManager.Create(model.Name, url, model.OwnerId, model.Users);
            return Ok();
        }

        [HttpPost]
        [Route("search")]
        public IActionResult Search([FromBody] string searchString)
        {
            var chats = chatManager.Chats.Include(c => c.Messages).Where(c => c.Name.Contains(searchString))
                .Select(c => new ChatPreview(c.Id, c.LogoUrl, c.Name, "Last message"));
            return Ok(chats);
        }

        [HttpGet]
        [Route("{chatId}/info")]
        public IActionResult Info(Guid chatId)
        {
            Chat chat = chatManager.Chats.Include(c => c.Users).Include(c => c.Messages).First(c => c.Id == chatId);
            ChatInfo info = new ChatInfo(chat.Name, chat.LogoUrl, chat.OwnerId, chat.Messages, chat.Users);
            return Ok(info);
        }
    }
}
