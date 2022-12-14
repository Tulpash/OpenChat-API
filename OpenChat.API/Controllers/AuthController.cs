using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.Interfaces;
using OpenChat.API.Models;
using OpenChat.API.DTO;
using System.Security.Claims;

namespace OpenChat.API.Controllers
{
    [Route("auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ChatUser> userManager;
        private readonly IJwtConfiguration jwt;

        public AuthController(UserManager<ChatUser> userManager, IJwtConfiguration jwt)
        {
            this.userManager = userManager;
            this.jwt = jwt;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Login([FromBody] DTO.Login model)
        {
            //Validate params
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound($"User with email: {model.Email} not found");
            }
            //Chek user password
            var checkPass = await userManager.CheckPasswordAsync(user, model.Password);
            if (!checkPass)
            {
                return BadRequest("Wrong password");
            }
            //Create claims
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName)
            };
            //Create token
            string token = jwt.CreateToken(claims);
            return Ok(new UserInfo(user.Id, user.Email, user.FirstName, user.LastName, token, user.UniqueName));
            //return Ok(new { Id = user.Id, Login = user.Email, FirstName = user.FirstName, LastName = user.LastName, Token = token, Unique = user.UniqueName });
        }
    }
}
