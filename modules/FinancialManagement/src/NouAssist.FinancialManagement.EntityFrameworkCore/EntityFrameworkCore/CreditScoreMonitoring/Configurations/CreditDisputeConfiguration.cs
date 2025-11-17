using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CreditScoreMonitoring.EntityFrameworkCore.Configurations
{
    public class CreditDisputeConfiguration : IEntityTypeConfiguration<CreditDispute>
    {
        public void Configure(EntityTypeBuilder<CreditDispute> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                CreditScoreMonitoringDbProperties.DbTablePrefix + "CreditDisputes",
                CreditScoreMonitoringDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Bureau)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ItemType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.ItemDescription)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.DisputeReason)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.SupportingDocumentation)
                .HasMaxLength(2000);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.SubmittedDate)
                .IsRequired();

            builder.Property(x => x.ResolutionNotes)
                .HasMaxLength(2000);

            builder.Property(x => x.ConfirmationNumber)
                .HasMaxLength(100);

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.Status });
            builder.HasIndex(x => x.SubmittedDate);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
