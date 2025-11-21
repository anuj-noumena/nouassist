using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for ValidationAttempt entity
    /// </summary>
    public class ValidationAttemptConfiguration : IEntityTypeConfiguration<ValidationAttempt>
    {
        public void Configure(EntityTypeBuilder<ValidationAttempt> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(ProductDiscoveryDbProperties.DbTablePrefix + "ValidationAttempts", 
                           ProductDiscoveryDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.HypothesisId)
                .IsRequired();

            builder.Property(x => x.Method)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxValidationNotesLength);

            builder.Property(x => x.PlannedStartDate)
                .IsRequired();

            builder.Property(x => x.ActualStartDate)
                .IsRequired(false);

            builder.Property(x => x.CompletedDate)
                .IsRequired(false);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Results)
                .HasMaxLength(ProductDiscoveryConsts.MaxValidationNotesLength);

            builder.Property(x => x.Notes)
                .HasMaxLength(ProductDiscoveryConsts.MaxValidationNotesLength);

            // Configure ValidationMetric collection as JSON
            builder.Property(x => x.Metrics)
                .HasConversion(
                    v => JsonSerializer.Serialize(v.Select(m => new { 
                        m.Name, m.Description, m.TargetValue, m.ActualValue, m.Unit 
                    }), JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<List<dynamic>>(v, JsonSerializerOptions.Default)!
                        .Select(item => new ValidationMetric(
                            item.GetProperty("Name").GetString()!,
                            item.GetProperty("Description").GetString()!,
                            item.GetProperty("TargetValue").GetDecimal(),
                            item.GetProperty("Unit").GetString()!,
                            item.GetProperty("ActualValue").GetDecimal()))
                        .ToList())
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.HypothesisId);
            builder.HasIndex(x => x.Method);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.PlannedStartDate);
            builder.HasIndex(x => x.CompletedDate);
        }
    }
}
