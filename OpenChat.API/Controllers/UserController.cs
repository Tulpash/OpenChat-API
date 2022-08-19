using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.Models;
using OpenChat.API.DTO;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Create([FromBody] NewUser model)
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
                FullName = $"{model.FirstName} {model.LastName}",
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
        [Route("search")]
        [AllowAnonymous]
        public IActionResult Search([FromBody] string searchString)
        {
            IEnumerable<UserPreview> users = userManager.Users
                .Where(u => u.FullName.ToLower().Contains(searchString.ToLower()))
                .Select(u => new UserPreview(u.Id, u.FullName, u.UniqueName));
            return Ok(users);
        }

        [HttpPost]
        [Route("{userId}/edit")]
        public async Task<IActionResult> Edit(string userId, [FromBody] EditUser model)
        {
            ChatUser user = await userManager.Users.Where(u => u.Id == userId).FirstAsync();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.FullName = $"{model.FirstName} {model.LastName}";
            user.Email = model.Email;
            user.UserName = model.Email;
            await userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpPost]
        [Route("{userId}/chats")]
        public async Task<IActionResult> Chats(string userId, [FromBody] string searchString)
        {
            ChatUser user = await userManager.Users.Where(u => u.Id == userId).Include(u => u.Chats).FirstAsync();
            var chats = user.Chats?
                .Where(c => c.Name.ToLower().Contains(searchString.ToLower()))
                .Select(c => new ChatPreview(c.Id, c.LogoUrl, c.Name, "Last message")).ToArray() ?? Array.Empty<ChatPreview>();
            return Ok(chats);
        }      
    }
}
