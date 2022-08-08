using OpenChat.API.Models;

namespace OpenChat.API.Interfaces
{
    public interface IConnectionManager
    {
        public void AddConnection(string? userName, string connectionId);

        public void RemoveConnection(string connectionId);

        public List<ChatUser> Users { get; } 
    }
}
