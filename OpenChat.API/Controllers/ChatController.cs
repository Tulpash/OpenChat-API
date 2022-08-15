using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.Data;
using OpenChat.API.DTO;
using OpenChat.API.Models;

namespace OpenChat.API.Controllers
{
    [Route("chat")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly UserManager<ChatUser> userManager;
        private readonly MainContext mainContext;

        public ChatController(UserManager<ChatUser> userManager, MainContext mainContext)
        {
            this.userManager = userManager;
            this.mainContext = mainContext;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateChat([FromBody] NewChat model)
        {
            ChatUser[] users = model.Users?.Select(x => userManager.Users.First(u => u.Id == x)).ToArray() ?? Array.Empty<ChatUser>();
            Chat chat = new Chat()
            {
                Name = model.Name,
                Users = users,
                Messages = Array.Empty<ChatMessage>(),
            };
            mainContext.Chats.Add(chat);
            await mainContext.SaveChangesAsync();
            return Ok("Chat created");
        }

        [HttpPost]
        [Route("search")]
        public IActionResult Search([FromBody] string searchString)
        {
            var chats = mainContext.Chats.Where(c => c.Name.Contains(searchString))
                .Select(c => new ChatPreview() { Id = c.Id, Name = c.Name, LastMessage = "Last message" })
                .AsEnumerable();
            return Ok(chats);
        }
    }
}
