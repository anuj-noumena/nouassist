using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;
using NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Text.Json;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore.Configurations
{
    public class AnalyzedRequirementConfiguration : IEntityTypeConfiguration<AnalyzedRequirement>
    {
        public void Configure(EntityTypeBuilder<AnalyzedRequirement> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(RequirementsAnalysisToolsDbProperties.DbTablePrefix + "AnalyzedRequirements",
                           RequirementsAnalysisToolsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.RequirementId)
                .IsRequired()
                .HasColumnName(nameof(AnalyzedRequirement.RequirementId));

            builder.Property(x => x.RequirementTitle)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName(nameof(AnalyzedRequirement.RequirementTitle));

            builder.Property(x => x.LastAnalyzedAt)
                .HasColumnName(nameof(AnalyzedRequirement.LastAnalyzedAt));

            builder.Property(x => x.LastAnalyzedBy)
                .HasColumnName(nameof(AnalyzedRequirement.LastAnalyzedBy));

            // Value Object: ImpactAssessmentResult
            builder.OwnsOne(x => x.ImpactAssessment, impactBuilder =>
            {
                impactBuilder.Property(i => i.TechnicalImpact)
                    .HasColumnName("TechnicalImpact")
                    .HasConversion<string>();

                impactBuilder.Property(i => i.BusinessImpact)
                    .HasColumnName("BusinessImpact")
                    .HasConversion<string>();

                impactBuilder.Property(i => i.UserImpact)
                    .HasColumnName("UserImpact")
                    .HasConversion<string>();

                impactBuilder.Property(i => i.RiskImpact)
                    .HasColumnName("RiskImpact")
                    .HasConversion<string>();

                impactBuilder.Property(i => i.Description)
                    .HasColumnName("ImpactDescription")
                    .HasMaxLength(AnalysisConsts.MaxImpactDescriptionLength);

                impactBuilder.Property(i => i.OverallScore)
                    .HasColumnName("ImpactOverallScore")
                    .HasColumnType("decimal(5,2)");
            });

            // Relationships
            builder.HasMany(x => x.Attributes)
                .WithOne()
                .HasForeignKey("AnalyzedRequirementId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Relationships)
                .WithOne()
                .HasForeignKey("SourceRequirementId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.RequirementId)
                .IsUnique()
                .HasDatabaseName("IX_AnalyzedRequirements_RequirementId");

            builder.HasIndex(x => x.LastAnalyzedAt)
                .HasDatabaseName("IX_AnalyzedRequirements_LastAnalyzedAt");

            // Multi-tenancy
            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_AnalyzedRequirements_TenantId");

            // Extra Properties and Concurrency
            builder.ConfigureExtraProperties();
            builder.ConfigureConcurrencyStamp();
        }
    }
}
