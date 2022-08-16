using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.Interfaces;
using OpenChat.API.Managers;
using OpenChat.API.Models;
using System.Security.Cryptography;

namespace OpenChat.API.Controllers
{
    [Route("test")]
    [ApiController]
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        private readonly IConnectionManager connectionManager;
        private readonly UserManager<ChatUser> userManager;


        public TestController(IConnectionManager connectionManager, UserManager<ChatUser> userManager)
        {
            this.connectionManager = connectionManager;
            this.userManager = userManager;
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

        [HttpPost]
        [Route("def")]
        public async Task<IActionResult> Def()
        {
            string[] fNames = { "Роман", "Иван", "Анна", "Сергей", "Дмитрий", "Юрий", "Саша", "Валерия", "София", "Анастасия", "Лада", "Лиза", "Елизавета", "Александар", "Александра", "Павел", "Семен", "Сема" };
            string[] lNames = { "Ивакин", "Ивакина", "Парылина", "Парылин", "Криник", "Шпигель", "Загородная", "Загородный", "Гурин", "Гурина", "Морозов", "Морозова" };
            Random rnd = new Random();

            for (int i = 0; i < 100; i++)
            {
                int tu = RandomNumberGenerator.GetInt32(100000000, 999999999);
                string unique = $"@{tu}";
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
