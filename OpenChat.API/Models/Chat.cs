namespace OpenChat.API.Models
{
    public class Chat
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        //Users ref
        public ChatUser[] Users { get; set; } = null!;

        //Messages ref
        public ChatMessage[] Messages { get; set; } = null!;
    }
}
