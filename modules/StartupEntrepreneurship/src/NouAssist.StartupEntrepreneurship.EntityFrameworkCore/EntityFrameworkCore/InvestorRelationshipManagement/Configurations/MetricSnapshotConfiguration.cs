using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for MetricSnapshot entity
    /// </summary>
    public class MetricSnapshotConfiguration : IEntityTypeConfiguration<MetricSnapshot>
    {
        public void Configure(EntityTypeBuilder<MetricSnapshot> builder)
        {
            builder.ToTable(IRMDbProperties.DbTablePrefix + "MetricSnapshots", IRMDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UpdateId)
                .IsRequired();

            builder.Property(x => x.SnapshotDate)
                .IsRequired();

            // Value Object Mapping - MetricValue
            builder.OwnsOne(x => x.Metric, metricBuilder =>
            {
                metricBuilder.Property(m => m.Name)
                    .IsRequired()
                    .HasColumnName("MetricName")
                    .HasMaxLength(200);

                metricBuilder.Property(m => m.Value)
                    .IsRequired()
                    .HasColumnName("MetricValue")
                    .HasColumnType("decimal(18,4)");

                metricBuilder.Property(m => m.Unit)
                    .HasColumnName("MetricUnit")
                    .HasMaxLength(50);

                metricBuilder.Property(m => m.PreviousValue)
                    .HasColumnName("MetricPreviousValue")
                    .HasColumnType("decimal(18,4)");

                metricBuilder.Property(m => m.TargetValue)
                    .HasColumnName("MetricTargetValue")
                    .HasColumnType("decimal(18,4)");
            });

            // Indexes
            builder.HasIndex(x => x.UpdateId)
                .HasDatabaseName("IX_MetricSnapshots_UpdateId");

            builder.HasIndex(x => x.SnapshotDate)
                .HasDatabaseName("IX_MetricSnapshots_SnapshotDate");

            builder.HasIndex("Metric_Name")
                .HasDatabaseName("IX_MetricSnapshots_MetricName");
        }
    }
}
