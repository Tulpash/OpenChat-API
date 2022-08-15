using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.Models;
using OpenChat.API.RequestModels;
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
        public async Task<IActionResult> Create([FromBody] NewUser model)
        {
            //Create unique name
            string unique = $"#{RandomNumberGenerator.GetInt32(100000000, 999999999)}";
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

        [HttpGet]
        [Route("chats")]
        public IActionResult GetChats(Guid userId)
        {
            return Ok(Enumerable.Empty<object>());
        }

        [HttpPost]
        [Route("def")]
        [AllowAnonymous]
        public async Task<IActionResult> Def()
        {
            string[] fNames = { "Роман", "Иван", "Анна", "Сергей", "Дмитрий", "Юрий", "Саша", "Валерия", "София", "Анастасия", "Лада", "Лиза", "Елизавета", "Александар", "Александра", "Павел", "Семен", "Сема" };
            string[] lNames = { "Ивакин", "Ивакина", "Парылина", "Парылин", "Криник", "Шпигель", "Загородная", "Загородный", "Гурин", "Гурина", "Морозов", "Морозова" };
            Random rnd = new Random();

            for (int i = 0; i < 100; i++)
            {
                int tu = RandomNumberGenerator.GetInt32(100000000, 999999999);
                string unique = $"#{tu}";
                var count = userManager.Users.Count(u => u.UniqueName == unique);
                if (count > 0)
                {
                    return BadRequest("Error when create unique name");
                }
                string email = $"test_{tu}@mail.com";
                ChatUser user = new ChatUser()
                {
                    FirstName = fNames[rnd.Next(0, fNames.Length)],
                    LastName = lNames[rnd.Next(0, lNames.Length)],
                    Email = email,
                    UserName = email,
                    UniqueName = unique
                };
                var res = await userManager.CreateAsync(user, "_Roman343");
                if (!res.Succeeded)
                {
                    return BadRequest(res.Errors.Select(e => e.Description));
                }
            }

            return Ok();
        }
    }
}
