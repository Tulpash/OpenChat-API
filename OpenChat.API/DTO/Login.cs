using OpenChat.API.Models;

namespace OpenChat.API.DTO
{
    public record Login(string Email, string Password);

    public record NewChat(IFormFile? Logo, string Name, string[] Users);

    public record NewUser(string FirstName, string LastName, string Email, string Password);

    public record ChatPreview(Guid Id, string LogoUrl, string Name, string? LastMessage);

    public record UserPreview(string Id, string? FullName, string? Unique);

    public record ChatInfo(string Name, string LogoUrl, IEnumerable<ChatMessage> Messages, IEnumerable<ChatUser> Users);
}
