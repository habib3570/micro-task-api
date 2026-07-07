using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroTaskAPI.Domain.Entities;

namespace MicroTaskAPI.Infrastructure.Persistence.Configurations
{
    public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
    {
        public void Configure(EntityTypeBuilder<Submission> builder)
        {
            builder.ToTable("Submissions");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.TaskTitle).IsRequired().HasMaxLength(250);
            builder.Property(s => s.PayableAmount).IsRequired();
            builder.Property(s => s.WorkerName).IsRequired().HasMaxLength(150);
            builder.Property(s => s.BuyerName).IsRequired().HasMaxLength(150);
            builder.Property(s => s.SubmissionDetail).IsRequired();
            builder.Property(s => s.CurrentDate).IsRequired();

            builder.Property(s => s.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(s => s.Task)
                .WithMany(t => t.Submissions)
                .HasForeignKey(s => s.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Worker)
                .WithMany(u => u.WorkerSubmissions)
                .HasForeignKey(s => s.WorkerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Buyer)
                .WithMany()
                .HasForeignKey(s => s.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}