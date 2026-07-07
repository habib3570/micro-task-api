using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroTaskAPI.Domain.Entities;

namespace MicroTaskAPI.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Message).IsRequired();
            builder.Property(n => n.ActionRoute).HasMaxLength(300);
            builder.Property(n => n.Time).IsRequired();
            builder.Property(n => n.IsRead).IsRequired();

            builder.HasOne(n => n.ToUser)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}