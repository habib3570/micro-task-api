using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroTaskAPI.Domain.Entities;

namespace MicroTaskAPI.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.DisplayName).IsRequired().HasMaxLength(150);

            builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.PhotoUrl).HasMaxLength(500);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(u => u.Coin).IsRequired();

            builder.Property(u => u.FirebaseUid).HasMaxLength(200);
            builder.HasIndex(u => u.FirebaseUid).IsUnique(false);

            builder.HasMany(u => u.Tasks)
                .WithOne(t => t.Buyer)
                .HasForeignKey(t => t.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.WorkerSubmissions)
                .WithOne(s => s.Worker)
                .HasForeignKey(s => s.WorkerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Payments)
                .WithOne(p => p.Buyer)
                .HasForeignKey(p => p.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Withdrawals)
                .WithOne(w => w.Worker)
                .HasForeignKey(w => w.WorkerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Notifications)
                .WithOne(n => n.ToUser)
                .HasForeignKey(n => n.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}