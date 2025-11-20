using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for InvestorUpdate aggregate
    /// </summary>
    public class InvestorUpdateConfiguration : IEntityTypeConfiguration<InvestorUpdate>
    {
        public void Configure(EntityTypeBuilder<InvestorUpdate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(IRMDbProperties.DbTablePrefix + "InvestorUpdates", IRMDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(IRMConsts.InvestorUpdate.MaxTitleLength);

            builder.Property(x => x.Subject)
                .IsRequired()
                .HasMaxLength(IRMConsts.InvestorUpdate.MaxSubjectLength);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Frequency)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.TemplateId)
                .HasMaxLength(100);

            // One-to-Many Relationships within aggregate boundary
            builder.HasMany<UpdateSection>()
                .WithOne()
                .HasForeignKey("UpdateId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<MetricSnapshot>()
                .WithOne()
                .HasForeignKey("UpdateId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<DistributionLog>()
                .WithOne()
                .HasForeignKey("UpdateId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<EngagementRecord>()
                .WithOne()
                .HasForeignKey("UpdateId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_InvestorUpdates_Status");

            builder.HasIndex(x => x.ScheduledDate)
                .HasDatabaseName("IX_InvestorUpdates_ScheduledDate");

            builder.HasIndex(x => x.SentDate)
                .HasDatabaseName("IX_InvestorUpdates_SentDate");

            builder.HasIndex(x => x.Frequency)
                .HasDatabaseName("IX_InvestorUpdates_Frequency");

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(InvestorUpdate.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
