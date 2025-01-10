using Microsoft.EntityFrameworkCore;
using UWPMessenger.Models;

namespace UWPMessenger.Data
{
    public class MessageAppContext : DbContext
    {
        public MessageAppContext(DbContextOptions<MessageAppContext> options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<SentMessage> SentMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasMany(m => m.SentMessages)
                .WithOne(sm => sm.Message)
                .HasForeignKey(sm => sm.MessageId);
        }
    }
}
