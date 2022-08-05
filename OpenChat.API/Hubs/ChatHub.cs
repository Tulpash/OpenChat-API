using Microsoft.AspNetCore.SignalR;
using OpenChat.API.Managers;
using OpenChat.API.Interfaces;

namespace OpenChat.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatManager chatManager;

        public ChatHub(ChatManager chatManager)
        {
            this.chatManager = chatManager;
        }

        public override Task OnConnectedAsync()
        {
            chatManager.AddConnection(Context.User?.Identity?.Name, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            chatManager.RemoveConnection(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
