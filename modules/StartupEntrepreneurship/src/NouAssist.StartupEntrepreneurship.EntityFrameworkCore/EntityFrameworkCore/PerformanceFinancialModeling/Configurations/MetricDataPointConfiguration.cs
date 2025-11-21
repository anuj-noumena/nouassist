using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Configurations
{
    public class MetricDataPointConfiguration : IEntityTypeConfiguration<MetricDataPoint>
    {
        public void Configure(EntityTypeBuilder<MetricDataPoint> builder)
        {
            builder.ToTable(PerformanceFinancialModelingDbProperties.DbTablePrefix + "MetricDataPoints", 
                           PerformanceFinancialModelingDbProperties.DbSchema);

            builder.HasKey(x => new { x.MetricId, x.Timestamp });

            builder.Property(x => x.MetricId)
                .IsRequired();

            builder.Property(x => x.Timestamp)
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxNotesLength);

            // Value Object Mapping - Value
            builder.OwnsOne(x => x.Value, valueBuilder =>
            {
                valueBuilder.Property(v => v.Value)
                    .HasColumnName("Value")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                valueBuilder.Property(v => v.DataType)
                    .HasColumnName("DataType")
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

                valueBuilder.Property(v => v.Unit)
                    .HasColumnName("Unit")
                    .HasMaxLength(20);
            });

            // Indexes
            builder.HasIndex(x => x.MetricId);
            builder.HasIndex(x => x.Timestamp);
            builder.HasIndex(x => new { x.MetricId, x.Timestamp }).IsUnique();
        }
    }
}
