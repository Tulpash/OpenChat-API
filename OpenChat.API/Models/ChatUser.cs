using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenChat.API.Models
{
    public class ChatUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        //Chats ref
        public Chat[] Chats { get; set; } = null!;

        //SignalR connections ref
        [NotMapped]
        public List<ChatConnection> Connections { get; set; } = null!;
    }
}
