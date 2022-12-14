using Microsoft.AspNetCore.Identity;
using OpenChat.API.Interfaces;
using OpenChat.API.Models;

namespace OpenChat.API.Managers
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly UserManager<ChatUser> userManager;
        private static readonly List<ChatUser> users = null!;

        static ConnectionManager()
        {
            users = new List<ChatUser>();
        }
        
        public ConnectionManager(UserManager<ChatUser> userManager)
        {
            this.userManager = userManager;
        }

        public IQueryable<ChatUser> Users => users.AsQueryable();

        public void AddConnection(string? userName, string connectionId)
        {
            _ = userName ?? throw new ArgumentNullException($"{nameof(userName)} was null");
            _ = connectionId ?? throw new ArgumentNullException($"{nameof(connectionId)} was null");

            var user = users.FirstOrDefault(u => u.Id == userName);

            if (user == null)
            {
                user = userManager.FindByNameAsync(userName).Result;
                users.Add(user);
            }

            if (user.Connections == null) user.Connections = new List<ChatConnection>();

            user.Connections.Add(new ChatConnection() { Id = connectionId, ConnectedAt = DateTime.Now });
        }

        public void RemoveConnection(string connectionId)
        {
            _ = connectionId ?? throw new ArgumentNullException($"{nameof(connectionId)} was null");

            var user = users.FirstOrDefault(u => u.Connections.Count(c => c.Id == connectionId) > 0);

            _ = user ?? throw new NullReferenceException($"User with connection {connectionId}");

            var connection = user.Connections.First(c => c.Id == connectionId);
            user.Connections.Remove(connection);

            if (user.Connections.Count == 0) users.Remove(user);
        }


    }
}
