using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Text.Json;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Configurations
{
    public class ImpactMetricDefinitionConfiguration : IEntityTypeConfiguration<ImpactMetricDefinition>
    {
        public void Configure(EntityTypeBuilder<ImpactMetricDefinition> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(BusinessModelManagementDbProperties.DbTablePrefix + "ImpactMetricDefinitions",
                           BusinessModelManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.PivotId)
                .IsRequired();

            builder.Property(x => x.MetricName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Unit)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.MeasurementMethod)
                .HasMaxLength(500);

            builder.Property(x => x.BaselineValue)
                .HasColumnType("decimal(18,4)");

            builder.Property(x => x.TargetValue)
                .HasColumnType("decimal(18,4)");

            // Value Object Configuration - ImpactValues (stored as JSON collection)
            builder.Property(x => x.Measurements)
                .HasColumnName("MeasurementsJson")
                .HasConversion(
                    v => JsonSerializer.Serialize(v.Select(m => new
                    {
                        m.MetricName,
                        m.Value,
                        m.Unit,
                        m.MeasuredAt,
                        m.MeasurementMethod,
                        m.Notes
                    }), (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<dynamic>>(v, (JsonSerializerOptions)null)
                        .Select(m => new ImpactValue(
                            m.GetProperty("MetricName").GetString(),
                            m.GetProperty("Value").GetDecimal(),
                            m.GetProperty("Unit").GetString(),
                            m.GetProperty("MeasuredAt").GetDateTime(),
                            m.TryGetProperty("MeasurementMethod", out var method) && method.ValueKind != JsonValueKind.Null 
                                ? method.GetString() : null,
                            m.TryGetProperty("Notes", out var notes) && notes.ValueKind != JsonValueKind.Null 
                                ? notes.GetString() : null))
                        .ToList())
                .HasMaxLength(8000);

            // Indexes
            builder.HasIndex(x => x.PivotId);
            builder.HasIndex(x => new { x.PivotId, x.MetricName }).IsUnique();
        }
    }
}
