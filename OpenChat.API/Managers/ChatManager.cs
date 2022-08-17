using OpenChat.API.Data;
using OpenChat.API.Interfaces;
using OpenChat.API.Models;

namespace OpenChat.API.Managers
{
    public class ChatManager : IChatManager
    {
        private readonly MainContext context;

        public ChatManager(MainContext context)
        {
            this.context = context;
        }

        public bool Create(string name, string logoUrl, ChatUser[] users)
        {
            _ = name ?? throw new ArgumentNullException($"{nameof(name)} was null");
            _ = logoUrl ?? throw new ArgumentNullException($"{nameof(logoUrl)} was null");
            _ = users ?? throw new ArgumentNullException($"{nameof(users)} was null");

            return true;
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
