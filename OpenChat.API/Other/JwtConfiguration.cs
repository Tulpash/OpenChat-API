using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OpenChat.API.Other
{
    public static class JwtConfiguration
    {
        //Options
        private static bool validateAudience = true;
        private static string audience = "www.OpenChat.com";
        private static bool validateIssuer = true;
        private static string issuer = "www.OpenChat.com";
        private static TimeSpan clockSkew = TimeSpan.Zero;
        private static bool validateLifetime = true;
        private static int lifetime = 7;
        private static bool validateKey = true;
        private static string key = "eShVmYq3t6v9y$B&E)H@McQfTjWnZr4u";
        
        //Key to SecurityKey
        private static SymmetricSecurityKey GetKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        
        /// <summary>
        /// Create validation parameters
        /// </summary>
        /// <returns><see cref="TokenValidationParameters"/></returns>
        public static TokenValidationParameters ValidationParameters()
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
        public static string CreateToken(IList<Claim> claims)
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
