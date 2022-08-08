using Microsoft.AspNetCore.SignalR;
using OpenChat.API.Managers;
using OpenChat.API.Interfaces;

namespace OpenChat.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IConnectionManager connectionManager;

        public ChatHub(IConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        public override Task OnConnectedAsync()
        {
            connectionManager.AddConnection(Context.User?.Identity?.Name, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            connectionManager.RemoveConnection(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
