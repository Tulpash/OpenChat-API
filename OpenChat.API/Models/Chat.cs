namespace OpenChat.API.Models
{
    public class Chat
    {
        public Guid Id { get; }

        public string LogoUrl { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string OwnerId { get; set; } = null!;

        //Users ref
        public List<ChatUser> Users { get; set; } = null!;

        //Messages ref
        public List<ChatMessage> Messages { get; set; } = null!;
    }
}
