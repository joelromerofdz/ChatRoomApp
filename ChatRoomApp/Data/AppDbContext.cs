using ChatRoomApp.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomApp.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne<User>(m => m.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>(m => m.Receiver)
                .WithMany(u => u.MessagesReceiver)
                .HasForeignKey(m => m.ReceiverId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public DbSet<Message> Messages { get; set; }
    }
}
