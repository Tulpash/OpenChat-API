using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.Data;
using OpenChat.API.DTO;
using OpenChat.API.Models;

namespace OpenChat.API.Controllers
{
    [Route("chats")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly UserManager<ChatUser> userManager;
        private readonly MainContext mainContext;
        private readonly IWebHostEnvironment env;

        public ChatController(UserManager<ChatUser> userManager, MainContext mainContext, IWebHostEnvironment env)
        {
            this.userManager = userManager;
            this.mainContext = mainContext;
            this.env = env;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateChat([FromForm] NewChat model)
        {
            string logoUrl = $"https://localhost:7236/ChatsLogo/Default.png";
            if (model.Logo != null)
            {
                string type = model.Logo.FileName.Substring(model.Logo.FileName.LastIndexOf('.') + 1);
                string fileName = $"{Guid.NewGuid()}.{type}";
                string path = Path.Combine(env.WebRootPath, "ChatsLogo", fileName);
                using (FileStream fs = System.IO.File.Create(path))
                {
                    await model.Logo.CopyToAsync(fs);
                    fs.Close();
                }
                logoUrl = $"https://localhost:7236/ChatsLogo/{fileName}";
            }
            ChatUser[] users = model.Users?.Select(x => userManager.Users.First(u => u.Id == x)).ToArray() ?? Array.Empty<ChatUser>();
            Chat chat = new Chat()
            {
                LogoUrl = logoUrl,
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
                .Select(c => new ChatPreview() { Id = c.Id, LogoUrl = c.LogoUrl, Name = c.Name, LastMessage = "Last message" })
                .AsEnumerable();
            return Ok(chats);
        }
    }
}
