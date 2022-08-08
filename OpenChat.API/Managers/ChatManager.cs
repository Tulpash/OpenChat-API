using OpenChat.API.Data;

namespace OpenChat.API.Managers
{
    public class ChatManager
    {
        private readonly MainContext context;

        public ChatManager(MainContext context)
        {
            this.context = context;
        }

        public void CreateChat(string name)
        {

        }
    }
}
