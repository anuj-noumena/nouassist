using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using NouAssist.FinancialManagement.DebtManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.DebtManagement.EntityFrameworkCore.Configurations
{
    public class DebtAccountConfiguration : IEntityTypeConfiguration<DebtAccount>
    {
        public void Configure(EntityTypeBuilder<DebtAccount> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                DebtManagementDbProperties.DbTablePrefix + "DebtAccounts",
                DebtManagementDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.DebtName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.AccountNumber)
                .HasMaxLength(100);

            builder.Property(x => x.CreditorName)
                .HasMaxLength(200);

            builder.Property(x => x.OriginalAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.CurrentBalance)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.InterestRate)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(x => x.MinimumPayment)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.PaymentFrequency)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Relationships - Payments (owned collection)
            builder.OwnsMany(x => x.Payments, payment =>
            {
                payment.ToTable(
                    DebtManagementDbProperties.DbTablePrefix + "DebtPayments",
                    DebtManagementDbProperties.DbSchema
                );

                payment.WithOwner().HasForeignKey(nameof(DebtAccount) + "Id");

                payment.Property<Guid>(nameof(DebtAccount) + "Id").IsRequired();
                
                payment.Property(p => p.PaymentDate).IsRequired();
                payment.Property(p => p.PaymentAmount).IsRequired().HasPrecision(18, 2);
                payment.Property(p => p.PrincipalAmount).IsRequired().HasPrecision(18, 2);
                payment.Property(p => p.InterestAmount).IsRequired().HasPrecision(18, 2);
                payment.Property(p => p.RemainingBalance).IsRequired().HasPrecision(18, 2);
                
                payment.Property(p => p.Status)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                payment.Property(p => p.TransactionReference).HasMaxLength(100);
                payment.Property(p => p.Notes).HasMaxLength(500);

                payment.HasKey(nameof(DebtAccount) + "Id", nameof(DebtPayment.PaymentDate));
            });

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => new { x.UserId, x.Status });
            builder.HasIndex(x => x.NextPaymentDueDate);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
