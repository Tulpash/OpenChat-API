﻿namespace OpenChat.API.RequestModels
{
    public class Login
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}