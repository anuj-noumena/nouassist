using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for DistributionLog entity
    /// </summary>
    public class DistributionLogConfiguration : IEntityTypeConfiguration<DistributionLog>
    {
        public void Configure(EntityTypeBuilder<DistributionLog> builder)
        {
            builder.ToTable(IRMDbProperties.DbTablePrefix + "DistributionLogs", IRMDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UpdateId)
                .IsRequired();

            builder.Property(x => x.InvestorId)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Channel)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ErrorMessage)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(x => x.UpdateId)
                .HasDatabaseName("IX_DistributionLogs_UpdateId");

            builder.HasIndex(x => x.InvestorId)
                .HasDatabaseName("IX_DistributionLogs_InvestorId");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_DistributionLogs_Status");

            builder.HasIndex(x => x.AttemptedAt)
                .HasDatabaseName("IX_DistributionLogs_AttemptedAt");

            builder.HasIndex(x => new { x.UpdateId, x.InvestorId })
                .HasDatabaseName("IX_DistributionLogs_UpdateId_InvestorId");
        }
    }
}
