using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.Models;
using OpenChat.API.DTO;
using System.Security.Cryptography;

namespace OpenChat.API.Controllers
{
    [Route("users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ChatUser> userManager;

        public UserController(UserManager<ChatUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] Registration model)
        {
            //Create unique name
            string unique = $"@{RandomNumberGenerator.GetInt32(100000000, 999999999)}";
            //Check unique (Незнаю как сгенирировать конкретно, но надо поправить)
            var count = userManager.Users.Count(u => u.UniqueName == unique); 
            if (count > 0)
            {
                return BadRequest("Error when create unique name");
            }
            //Create new user
            ChatUser user = new ChatUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                UniqueName = unique
            };
            var res = await userManager.CreateAsync(user, model.Password);
            if (!res.Succeeded)
            {
                return BadRequest(res.Errors.Select(e => e.Description));
            }
            return NoContent();
        }

        [HttpPost]
        [Route("{userId}/chats")]
        public async Task<IActionResult> Chats(string userId, [FromBody] string searchString)
        {
            ChatUser user = await userManager.FindByIdAsync(userId);
            var chats = user.Chats?.Where(c => c.Name.Contains(searchString))
                .Select(c => new ChatPreview() { Id = c.Id, LogoUrl = c.LogoUrl, Name = c.Name, LastMessage = "Last message" }) ?? Array.Empty<ChatPreview>();
            return Ok(chats);
        }

        [HttpPost]
        [Route("search")]
        [AllowAnonymous]
        public IActionResult Search([FromBody] string searchString)
        {
            IEnumerable<UserPreview> users = userManager.Users
                .Where(u => u.FirstName.Contains(searchString))
                .Select(u => new UserPreview() { Id = u.Id, FullName = u.FullName, Unique = u.UniqueName });
            return Ok(users);
        }
    }
}
