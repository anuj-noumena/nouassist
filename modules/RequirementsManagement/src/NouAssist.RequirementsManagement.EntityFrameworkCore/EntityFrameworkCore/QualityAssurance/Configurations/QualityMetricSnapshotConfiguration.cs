using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.QualityAssurance.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore.Configurations
{
    public class QualityMetricSnapshotConfiguration : IEntityTypeConfiguration<QualityMetricSnapshot>
    {
        public void Configure(EntityTypeBuilder<QualityMetricSnapshot> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(QADbProperties.DbTablePrefix + "QualityMetrics", QADbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Value Object Mappings
            builder.OwnsOne(x => x.Metric, metricBuilder =>
            {
                metricBuilder.Property(m => m.Type)
                    .HasColumnName("MetricType")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                metricBuilder.Property(m => m.Value)
                    .HasColumnName("MetricValue")
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                metricBuilder.Property(m => m.Unit)
                    .HasColumnName("MetricUnit")
                    .IsRequired()
                    .HasMaxLength(50);

                metricBuilder.Property(m => m.Timestamp)
                    .HasColumnName("MetricTimestamp")
                    .IsRequired();
            });

            // Property Mappings
            builder.Property(x => x.TestPlanId)
                .IsRequired(false);

            builder.Property(x => x.ReleaseVersion)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(x => x.Context)
                .IsRequired(false)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex("Metric_Type");
            builder.HasIndex("Metric_Timestamp");
            builder.HasIndex(x => x.TestPlanId);
            builder.HasIndex(x => x.ReleaseVersion);
        }
    }
}
