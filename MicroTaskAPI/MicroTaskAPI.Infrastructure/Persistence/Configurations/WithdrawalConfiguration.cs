using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroTaskAPI.Domain.Entities;

namespace MicroTaskAPI.Infrastructure.Persistence.Configurations
{
    public class WithdrawalConfiguration : IEntityTypeConfiguration<Withdrawal>
    {
        public void Configure(EntityTypeBuilder<Withdrawal> builder)
        {
            builder.ToTable("Withdrawals");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.WorkerName).IsRequired().HasMaxLength(150);
            builder.Property(w => w.WithdrawalCoin).IsRequired();
            builder.Property(w => w.WithdrawalAmount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(w => w.PaymentSystem).IsRequired().HasMaxLength(50);
            builder.Property(w => w.AccountNumber).IsRequired().HasMaxLength(30);
            builder.Property(w => w.WithdrawDate).IsRequired();
            builder.Property(w => w.RejectionReason).HasMaxLength(500);

            builder.Property(w => w.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(w => w.Worker)
                .WithMany(u => u.Withdrawals)
                .HasForeignKey(w => w.WorkerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}