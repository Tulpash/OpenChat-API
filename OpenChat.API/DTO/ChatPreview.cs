namespace OpenChat.API.DTO
{
    public class ChatPreview
    {
        public Guid Id { get; set; }

        public string LogoUrl { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? LastMessage { get; set; }
    }
}
