using OpenChat.API.Models;

namespace OpenChat.API.DTO
{
    public record Login(string Email, string Password);
    public record NewUser(string FirstName, string LastName, string Email, string Password);
    public record EditUser(string FirstName, string LastName, string Email);
    public record UserPreview(string Id, string? FullName, string? Unique);
    public record NewChat(IFormFile? Logo, string Name, string OwnerId, string[] Users);
    public record ChatPreview(Guid Id, string LogoUrl, string Name, string? LastMessage);
    public record ChatInfo(string Name, string LogoUrl, string OwnerId, IEnumerable<ChatMessage> Messages, IEnumerable<ChatUser> Users);
}
