using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for DataTransformationStep entity
    /// </summary>
    public class DataTransformationStepConfiguration : IEntityTypeConfiguration<DataTransformationStep>
    {
        public void Configure(EntityTypeBuilder<DataTransformationStep> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                AnalyticsReportingDbProperties.DbTablePrefix + "DataTransformationSteps",
                AnalyticsReportingDbProperties.DbSchema
            );

            // Primary key is handled by Entity<TKey>

            builder.Property(x => x.ExecutionOrder)
                .IsRequired();

            builder.Property(x => x.IsEnabled)
                .IsRequired();

            builder.Property(x => x.LastModified)
                .IsRequired();

            // Complex type configuration
            builder.OwnsOne(x => x.Rule, ruleBuilder =>
            {
                ruleBuilder.Property(r => r.Name).HasColumnName("TransformationName")
                    .HasMaxLength(AnalyticsReportingConsts.MaxNameLength);
                ruleBuilder.Property(r => r.SourceExpression).HasColumnName("SourceExpression")
                    .HasMaxLength(AnalyticsReportingConsts.MaxNameLength);
                ruleBuilder.Property(r => r.TargetField).HasColumnName("TargetField")
                    .HasMaxLength(AnalyticsReportingConsts.MaxNameLength);
                ruleBuilder.Property(r => r.TransformationExpression).HasColumnName("TransformationExpression")
                    .HasMaxLength(AnalyticsReportingConsts.MaxTransformationRuleLength);
                ruleBuilder.Property(r => r.ExecutionOrder).HasColumnName("RuleExecutionOrder");
            });

            // Indexes
            builder.HasIndex(x => x.DataSourceId);
            builder.HasIndex(x => x.ExecutionOrder);
        }
    }
}
