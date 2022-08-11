using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.Models;
using OpenChat.API.RequestModels;

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
            string uniqueRaw = string.Join("", Guid.NewGuid().ToByteArray());
            string unique = "#";
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                unique += uniqueRaw[random.Next(0, uniqueRaw.Length)];
            }
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
        [Route("search")]
        public IActionResult Search([FromBody] string search)
        {
            Random rnd = new Random();
            Func<ChatUser, bool> predicate = (user) => user.FirstName.Contains(search); ;
            IEnumerable<dynamic> users;
            if (search.Contains('#'))
            {
                predicate = (user) => user.UniqueName.Contains(search);
            }
            users = userManager.Users.Where(predicate).Select(u => new { Id = u.Id, Name = u.FullName, Unique = u.UniqueName, Notifications = rnd.Next(1000) }).ToList();
            return Ok(users);
        }
    }
}
