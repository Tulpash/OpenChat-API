using OpenChat.API.Models;

namespace OpenChat.API.Interfaces
{
    public interface IChatManager
    {
        public void Create(string name, string logoUrl, ChatUser[] users);

        public void Delete(Guid id);
    }
}
