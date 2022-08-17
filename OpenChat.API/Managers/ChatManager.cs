using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenChat.API.Data;
using OpenChat.API.Interfaces;
using OpenChat.API.Models;

namespace OpenChat.API.Managers
{
    public class ChatManager : IChatManager
    {
        private readonly MainContext mainContext;
        private readonly UserManager<ChatUser> userManager;

        public ChatManager(MainContext mainContext, UserManager<ChatUser> userManager)
        {
            this.mainContext = mainContext;
            this.userManager = userManager;
        }

        /// <summary>
        /// Return all chats as array
        /// </summary>
        public Chat[] Chats => mainContext.Chats.Include(c => c.Messages).ToArray();

        /// <summary>
        /// Create new chat
        /// </summary>
        /// <param name="name"></param>
        /// <param name="logoUrl"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public void Create(string name, string logoUrl, string[] userIds)
        {
            _ = name ?? throw new ArgumentNullException($"{nameof(name)} was null");
            _ = logoUrl ?? throw new ArgumentNullException($"{nameof(logoUrl)} was null");
            _ = userIds ?? throw new ArgumentNullException($"{nameof(userIds)} was null");

            List<ChatUser> users = userIds.Select(x => userManager.Users.First(u => u.Id == x)).ToList();
            Chat chat = new Chat()
            {
                LogoUrl = logoUrl,
                Name = name,
                Users = users,
                Messages = new List<ChatMessage>(),
            };
            mainContext.Chats.Add(chat);
            mainContext.SaveChanges();
        }

        /// <summary>
        /// Delete existing chat
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public void Delete(Guid id)
        {
            Chat? toDel = mainContext.Chats.Find(id);
            _ = toDel ?? throw new ArgumentException($"Wrong id");
            mainContext.Chats.Remove(toDel);
            mainContext.SaveChanges();
        }
    }
}
