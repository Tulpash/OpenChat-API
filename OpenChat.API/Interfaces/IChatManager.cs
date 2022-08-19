using OpenChat.API.Models;

namespace OpenChat.API.Interfaces
{
    public interface IChatManager
    {
        public IQueryable<Chat> Chats { get; } 

        public void Create(string name, string logoUrl, string ownerId, string[] userIds);

        public void Delete(Guid id);

        public ChatMessage AddTextMessage(Guid chatId, string senderId, string text);
    }
}
