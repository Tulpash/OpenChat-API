using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace OpenChat.API.Interfaces
{
    public interface IJwtConfiguration
    {
        public TokenValidationParameters ValidationParameters();

        public string CreateToken(IList<Claim> claims);
    }
}
