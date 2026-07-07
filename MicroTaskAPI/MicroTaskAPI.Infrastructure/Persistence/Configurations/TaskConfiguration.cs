using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroTaskAPI.Domain.Entities;

namespace MicroTaskAPI.Infrastructure.Persistence.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.TaskTitle).IsRequired().HasMaxLength(250);
            builder.Property(t => t.TaskDetail).IsRequired();
            builder.Property(t => t.RequiredWorkers).IsRequired();
            builder.Property(t => t.PayableAmount).IsRequired();
            builder.Property(t => t.CompletionDate).IsRequired();
            builder.Property(t => t.SubmissionInfo).IsRequired();
            builder.Property(t => t.TaskImageUrl).HasMaxLength(500);

            builder.HasOne(t => t.Buyer)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Submissions)
                .WithOne(s => s.Task)
                .HasForeignKey(s => s.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}