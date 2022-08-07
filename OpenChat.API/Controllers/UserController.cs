﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.Models;
using OpenChat.API.RequestModels;
using OpenChat.API.Other;
using System.Security.Claims;
using OpenChat.API.Interfaces;

namespace OpenChat.API.Controllers
{
    [Route("users")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ChatUser> userManager;
        private readonly IJwtConfiguration jwt;

        public UserController(UserManager<ChatUser> userManager, IJwtConfiguration jwt)
        {
            this.userManager = userManager;
            this.jwt = jwt;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] NewUser model)
        {
            //Create new user
            ChatUser user = new ChatUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };
            var res = await userManager.CreateAsync(user, model.Password);
            if (!res.Succeeded)
            {
                return BadRequest(res.Errors.Select(e => e.Description));
            }
            return NoContent();
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Login([FromBody] Login model)
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
            return Ok(new { Id = user.Id, Login = user.Email, FirstName = user.FirstName, LastName = user.LastName, Token = token });
        }
    }
}
