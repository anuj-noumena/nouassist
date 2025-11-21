using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Configurations
{
    public class TrackedMetricConfiguration : IEntityTypeConfiguration<TrackedMetric>
    {
        public void Configure(EntityTypeBuilder<TrackedMetric> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(PerformanceFinancialModelingDbProperties.DbTablePrefix + "TrackedMetrics", 
                           PerformanceFinancialModelingDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Basic Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxMetricNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxMetricDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.DataType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Unit)
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxMetricUnitLength);

            builder.Property(x => x.Frequency)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.DefinitionId)
                .IsRequired();

            builder.Property(x => x.LastUpdatedAt);

            // Value Object Mapping - LastValue
            builder.OwnsOne(x => x.LastValue, valueBuilder =>
            {
                valueBuilder.Property(v => v.Value)
                    .HasColumnName("LastValue")
                    .HasColumnType("decimal(18,2)");

                valueBuilder.Property(v => v.DataType)
                    .HasColumnName("LastValueDataType")
                    .HasConversion<string>()
                    .HasMaxLength(50);

                valueBuilder.Property(v => v.Unit)
                    .HasColumnName("LastValueUnit")
                    .HasMaxLength(20);
            });

            // One-to-Many: TrackedMetric -> MetricDataPoints
            builder.HasMany<MetricDataPoint>()
                .WithOne()
                .HasForeignKey(dp => dp.MetricId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: TrackedMetric -> MetricGoals
            builder.HasMany<MetricGoal>()
                .WithOne()
                .HasForeignKey(g => g.MetricId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.DefinitionId);
            builder.HasIndex(x => new { x.TenantId, x.Name }).IsUnique();

            // ExtraProperties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(TrackedMetric.ExtraProperties));

            // ConcurrencyStamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
