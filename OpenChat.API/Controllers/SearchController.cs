using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenChat.API.Models;

namespace OpenChat.API.Controllers
{
    [Route("search")]
    [ApiController]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly UserManager<ChatUser> userManager;

        public SearchController(UserManager<ChatUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("local")]
        public IActionResult SearchLocal([FromBody] string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return Ok(Enumerable.Empty<dynamic>());
            }
            Func<ChatUser, bool> predicate = (user) => user.FirstName.Contains(searchString) || user.LastName.Contains(searchString); ;
            if (searchString.Contains('#'))
            {
                predicate = (user) => user.UniqueName.Contains(searchString);
            }
            IEnumerable<dynamic> users = userManager.Users.Where(predicate).Select(u => new { Id = u.Id, Name = u.FullName, Unique = u.UniqueName }).ToList();
            return Ok(users);
        }

        [HttpPost]
        [Route("global")]
        public IActionResult SearchGlobal([FromBody] string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return Ok(Enumerable.Empty<dynamic>());
            }
            Func<ChatUser, bool> predicate = (user) => user.FirstName.Contains(searchString) || user.LastName.Contains(searchString); ;
            if (searchString.Contains('#'))
            {
                predicate = (user) => user.UniqueName.Contains(searchString);
            }
            IEnumerable<dynamic> users = userManager.Users.Where(predicate).Select(u => new { Id = u.Id, Name = u.FullName, Unique = u.UniqueName }).ToList();
            return Ok(users);
        }

        [HttpPost]
        [Route("messages")]
        public IActionResult SearchMessages([FromBody] string searchString)
        {
            return Ok(Enumerable.Empty<dynamic>());
        }
    }
}
