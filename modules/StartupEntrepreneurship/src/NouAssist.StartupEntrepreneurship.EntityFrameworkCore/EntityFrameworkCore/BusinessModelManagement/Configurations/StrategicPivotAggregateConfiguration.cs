using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Text.Json;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Configurations
{
    public class StrategicPivotAggregateConfiguration : IEntityTypeConfiguration<StrategicPivot>
    {
        public void Configure(EntityTypeBuilder<StrategicPivot> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(BusinessModelManagementDbProperties.DbTablePrefix + "StrategicPivots",
                           BusinessModelManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(BusinessModelManagementConsts.StrategicPivot.MaxDescriptionLength);

            builder.Property(x => x.InitiatedBy)
                .IsRequired();

            builder.Property(x => x.InitiatedDate)
                .IsRequired();

            builder.Property(x => x.TargetCompletionDate)
                .IsRequired();

            builder.Property(x => x.ActualCompletionDate);

            builder.Property(x => x.ActualOutcome)
                .HasMaxLength(BusinessModelManagementConsts.StrategicPivot.MaxActualOutcomeLength);

            builder.Property(x => x.WasSuccessful)
                .IsRequired();

            // Value Object Configuration - PivotRationale
            builder.OwnsOne(x => x.Rationale, rationaleBuilder =>
            {
                rationaleBuilder.Property(r => r.Reason)
                    .HasColumnName("RationaleReason")
                    .IsRequired()
                    .HasMaxLength(BusinessModelManagementConsts.StrategicPivot.MaxReasonLength);

                rationaleBuilder.Property(r => r.ExpectedOutcome)
                    .HasColumnName("RationaleExpectedOutcome")
                    .IsRequired()
                    .HasMaxLength(BusinessModelManagementConsts.StrategicPivot.MaxExpectedOutcomeLength);

                rationaleBuilder.Property(r => r.RiskAssessment)
                    .HasColumnName("RationaleRiskAssessment")
                    .HasMaxLength(2000);

                rationaleBuilder.Property(r => r.KeyInsights)
                    .HasColumnName("RationaleKeyInsights")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));

                rationaleBuilder.Property(r => r.DataPoints)
                    .HasColumnName("RationaleDataPoints")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));

                rationaleBuilder.Property(r => r.SuccessMetrics)
                    .HasColumnName("RationaleSuccessMetrics")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null));
            });

            // Relationships
            builder.HasMany(x => x.ImpactMetrics)
                .WithOne()
                .HasForeignKey("PivotId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.InitiatedBy);
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => new { x.TenantId, x.Type });
            builder.HasIndex(x => new { x.Status, x.TargetCompletionDate });

            // Multi-tenancy
            builder.Property(x => x.TenantId);

            // Extra Properties
            builder.Property(x => x.ExtraProperties).HasColumnName(nameof(StrategicPivot.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
