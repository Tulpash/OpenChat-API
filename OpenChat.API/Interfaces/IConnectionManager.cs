using OpenChat.API.Models;

namespace OpenChat.API.Interfaces
{
    public interface IConnectionManager
    {
        public IQueryable<ChatUser> Users { get; }

        public void AddConnection(string? userName, string connectionId);

        public void RemoveConnection(string connectionId);
    }
}
