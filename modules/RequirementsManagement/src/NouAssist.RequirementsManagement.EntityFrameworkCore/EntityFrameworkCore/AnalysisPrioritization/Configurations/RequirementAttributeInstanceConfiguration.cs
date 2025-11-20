using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Entities;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;
using NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects;
using System.Text.Json;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore.Configurations
{
    public class RequirementAttributeInstanceConfiguration : IEntityTypeConfiguration<RequirementAttributeInstance>
    {
        public void Configure(EntityTypeBuilder<RequirementAttributeInstance> builder)
        {
            builder.ToTable(RequirementsAnalysisToolsDbProperties.DbTablePrefix + "RequirementAttributeInstances",
                           RequirementsAnalysisToolsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.AnalyzedRequirementId)
                .IsRequired()
                .HasColumnName(nameof(RequirementAttributeInstance.AnalyzedRequirementId));

            builder.Property(x => x.AttributeName)
                .IsRequired()
                .HasMaxLength(AnalysisConsts.MaxAttributeNameLength)
                .HasColumnName(nameof(RequirementAttributeInstance.AttributeName));

            builder.Property(x => x.AttributeType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(RequirementAttributeInstance.AttributeType));

            builder.Property(x => x.CreatedTime)
                .IsRequired()
                .HasColumnName(nameof(RequirementAttributeInstance.CreatedTime));

            builder.Property(x => x.CreatedBy)
                .IsRequired()
                .HasColumnName(nameof(RequirementAttributeInstance.CreatedBy));

            // Value Object: AttributeValue
            builder.OwnsOne(x => x.AttributeValue, valueBuilder =>
            {
                valueBuilder.Property(v => v.StringValue)
                    .HasColumnName("AttributeStringValue")
                    .HasMaxLength(2000);

                valueBuilder.Property(v => v.NumericValue)
                    .HasColumnName("AttributeNumericValue")
                    .HasColumnType("decimal(18,6)");

                valueBuilder.Property(v => v.BooleanValue)
                    .HasColumnName("AttributeBooleanValue");

                valueBuilder.Property(v => v.DateTimeValue)
                    .HasColumnName("AttributeDateTimeValue");

                valueBuilder.Property(v => v.ValueType)
                    .HasColumnName("AttributeValueType")
                    .HasConversion<string>()
                    .HasMaxLength(50);
            });

            // Indexes
            builder.HasIndex(x => new { x.AnalyzedRequirementId, x.AttributeName })
                .IsUnique()
                .HasDatabaseName("IX_RequirementAttributeInstances_RequirementId_AttributeName");

            builder.HasIndex(x => x.AttributeType)
                .HasDatabaseName("IX_RequirementAttributeInstances_AttributeType");

            builder.HasIndex(x => x.CreatedTime)
                .HasDatabaseName("IX_RequirementAttributeInstances_CreatedTime");
        }
    }
}
