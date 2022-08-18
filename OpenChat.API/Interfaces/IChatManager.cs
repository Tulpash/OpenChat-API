using OpenChat.API.Models;

namespace OpenChat.API.Interfaces
{
    public interface IChatManager
    {
        public IQueryable<Chat> Chats { get; } 

        public void Create(string name, string logoUrl, string[] userIds);

        public void Delete(Guid id);

        public void AddTextMessage(ChatMessage message);
    }
}
