using OpenChat.API.Models;

namespace OpenChat.API.Interfaces
{
    public interface IChatManager
    {
        public Chat[] Chats { get; } 

        public void Create(string name, string logoUrl, string[] userIds);

        public void Delete(Guid id);
    }
}
