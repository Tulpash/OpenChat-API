using System.Text.Json.Serialization;

namespace OpenChat.API.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }

        public DateTime SendTime { get; set; }

        public string Text { get; set; } = null!;
        
        //Chat ref
        public Guid ChatId { get; set; }
        [JsonIgnore]
        public Chat Chat { get; set; } = null!;

        //User ref
        public string UserId { get; set; } = null!;
        [JsonIgnore]
        public ChatUser User { get; set; } = null!;
    }
}
