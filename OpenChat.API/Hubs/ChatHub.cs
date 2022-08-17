using Microsoft.AspNetCore.SignalR;
using OpenChat.API.Interfaces;
using OpenChat.API.Models;

namespace OpenChat.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IConnectionManager connectionManager;
        private readonly IChatManager chatManager;

        public ChatHub(IConnectionManager connectionManager, IChatManager chatManager)
        {
            this.connectionManager = connectionManager;
            this.chatManager = chatManager;
        }

        //
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

        //
        public async Task SendTextMessage(Guid chatId, string text)
        {
            Chat chat = chatManager.Chats.First(c => c.Id == chatId);
            ChatUser[] users = connectionManager.Users.IntersectBy<ChatUser, string>(chat.Users.Select(u => u.Id), u => u.Id).ToArray();
            var connections = users.SelectMany(u => u.Connections.Select(c => c.Id)).ToList();
            await Clients.Clients(connections).SendAsync("GetMessage", chatId, text);
        }
    }
}
