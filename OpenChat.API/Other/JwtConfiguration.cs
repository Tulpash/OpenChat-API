using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OpenChat.API.Interfaces;

namespace OpenChat.API.Other
{
    public class JwtConfiguration : IJwtConfiguration
    {
        //Options
        private bool validateAudience = true;
        private string audience = "www.OpenChat.com";
        private bool validateIssuer = true;
        private string issuer = "www.OpenChat.com";
        private TimeSpan clockSkew = TimeSpan.Zero;
        private bool validateLifetime = true;
        private int lifetime = 7;
        private bool validateKey = true;
        private string key = "eShVmYq3t6v9y$B&E)H@McQfTjWnZr4u";
        
        //Key to SecurityKey
        private SymmetricSecurityKey GetKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        
        /// <summary>
        /// Create validation parameters
        /// </summary>
        /// <returns><see cref="TokenValidationParameters"/></returns>
        public TokenValidationParameters ValidationParameters()
        {
            var parameters = new TokenValidationParameters()
            {
                ValidateAudience = validateAudience,
                ValidAudience = audience,
                ValidateIssuer = validateIssuer,
                ValidIssuer = issuer,
                ClockSkew = clockSkew,
                ValidateLifetime = validateLifetime,
                ValidateIssuerSigningKey = validateKey,
                IssuerSigningKey = GetKey()
            };
            return parameters;
        }

        /// <summary>
        /// Create token based on <paramref name="claims"/>
        /// </summary>
        /// <param name="claims"></param>
        /// <returns>Jwt token as string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string CreateToken(IList<Claim> claims)
        {
            _ = claims ?? throw new ArgumentNullException($"{nameof(claims)} was null");

            var now = DateTime.Now;
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromDays(lifetime)),
                signingCredentials: new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256)
                );
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }
    }
}
