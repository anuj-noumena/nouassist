using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for EngagementRecord entity
    /// </summary>
    public class EngagementRecordConfiguration : IEntityTypeConfiguration<EngagementRecord>
    {
        public void Configure(EntityTypeBuilder<EngagementRecord> builder)
        {
            builder.ToTable(IRMDbProperties.DbTablePrefix + "EngagementRecords", IRMDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UpdateId)
                .IsRequired();

            builder.Property(x => x.InvestorId)
                .IsRequired();

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Details)
                .HasMaxLength(1000);

            builder.Property(x => x.IpAddress)
                .HasMaxLength(45); // IPv6 support

            builder.Property(x => x.UserAgent)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(x => x.UpdateId)
                .HasDatabaseName("IX_EngagementRecords_UpdateId");

            builder.HasIndex(x => x.InvestorId)
                .HasDatabaseName("IX_EngagementRecords_InvestorId");

            builder.HasIndex(x => x.Type)
                .HasDatabaseName("IX_EngagementRecords_Type");

            builder.HasIndex(x => x.EngagedAt)
                .HasDatabaseName("IX_EngagementRecords_EngagedAt");

            builder.HasIndex(x => new { x.UpdateId, x.InvestorId, x.Type })
                .HasDatabaseName("IX_EngagementRecords_UpdateId_InvestorId_Type");
        }
    }
}
