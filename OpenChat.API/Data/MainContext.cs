using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenChat.API.Models;

namespace OpenChat.API.Data
{
    public class MainContext : IdentityDbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Chat> Chats { get; set; } = null!;
        public DbSet<ChatMessage> Messages { get; set; } = null!;
    }
}
