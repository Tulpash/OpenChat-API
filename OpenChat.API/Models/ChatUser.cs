using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenChat.API.Models
{
    public class ChatUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string UniqueName { get; set; } = null!;

        [JsonIgnore]
        //Chats ref
        public List<Chat> Chats { get; set; } = null!;

        //SignalR connections
        [NotMapped]
        [JsonIgnore]
        public List<ChatConnection> Connections { get; set; } = null!;
    }
}
