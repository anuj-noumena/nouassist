using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Text.Json;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore.Configurations
{
    public class PrioritizationActivityConfiguration : IEntityTypeConfiguration<PrioritizationActivity>
    {
        public void Configure(EntityTypeBuilder<PrioritizationActivity> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(RequirementsAnalysisToolsDbProperties.DbTablePrefix + "PrioritizationActivities",
                           RequirementsAnalysisToolsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AnalysisConsts.MaxPrioritizationActivityNameLength)
                .HasColumnName(nameof(PrioritizationActivity.Name));

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(AnalysisConsts.MaxPrioritizationActivityDescriptionLength)
                .HasColumnName(nameof(PrioritizationActivity.Description));

            builder.Property(x => x.Method)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(PrioritizationActivity.Method));

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(PrioritizationActivity.Status));

            builder.Property(x => x.StartDate)
                .HasColumnName(nameof(PrioritizationActivity.StartDate));

            builder.Property(x => x.EndDate)
                .HasColumnName(nameof(PrioritizationActivity.EndDate));

            builder.Property(x => x.AllowModification)
                .IsRequired()
                .HasColumnName(nameof(PrioritizationActivity.AllowModification));

            builder.Property(x => x.Instructions)
                .HasMaxLength(2000)
                .HasColumnName(nameof(PrioritizationActivity.Instructions));

            // Collections as JSON
            builder.Property(x => x.ParticipantIds)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null) ?? new List<Guid>())
                .HasColumnName("ParticipantIds")
                .HasColumnType("jsonb");

            builder.Property(x => x.RequirementIds)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null) ?? new List<Guid>())
                .HasColumnName("RequirementIds")
                .HasColumnType("jsonb");

            // Relationships
            builder.HasMany(x => x.Votes)
                .WithOne()
                .HasForeignKey("PrioritizationActivityId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_PrioritizationActivities_Status");

            builder.HasIndex(x => x.Method)
                .HasDatabaseName("IX_PrioritizationActivities_Method");

            builder.HasIndex(x => x.StartDate)
                .HasDatabaseName("IX_PrioritizationActivities_StartDate");

            builder.HasIndex(x => x.EndDate)
                .HasDatabaseName("IX_PrioritizationActivities_EndDate");

            // Multi-tenancy
            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_PrioritizationActivities_TenantId");

            // Extra Properties and Concurrency
            builder.ConfigureExtraProperties();
            builder.ConfigureConcurrencyStamp();
        }
    }
}
