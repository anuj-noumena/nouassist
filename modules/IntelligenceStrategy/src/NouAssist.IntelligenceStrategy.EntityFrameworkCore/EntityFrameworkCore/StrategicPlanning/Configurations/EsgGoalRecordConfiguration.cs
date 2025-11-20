using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for ESG Goal Record entity
    /// </summary>
    public class EsgGoalRecordConfiguration : IEntityTypeConfiguration<EsgGoalRecord>
    {
        public void Configure(EntityTypeBuilder<EsgGoalRecord> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "EsgGoalRecords", 
                            StrategicPlanningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Foreign Key
            builder.Property(x => x.SustainableGrowthFrameworkId)
                .IsRequired();

            // Enum Mapping
            builder.Property(x => x.Category)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Property Mappings
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(StrategicPlanningConsts.MaxGoalTitleLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(StrategicPlanningConsts.MaxEsgGoalDescriptionLength);

            builder.Property(x => x.CurrentValue)
                .HasColumnType("decimal(18,4)");

            builder.Property(x => x.LastMeasurementDate)
                .IsRequired(false);

            // Value Object Mapping - TargetMetric
            builder.OwnsOne(x => x.Target, targetBuilder => 
            {
                targetBuilder.Property(t => t.Name)
                    .HasColumnName("TargetMetricName")
                    .IsRequired()
                    .HasMaxLength(StrategicPlanningConsts.MaxMetricNameLength);
                
                targetBuilder.Property(t => t.Value)
                    .HasColumnName("TargetValue")
                    .IsRequired()
                    .HasColumnType("decimal(18,4)");

                targetBuilder.Property(t => t.Unit)
                    .HasColumnName("TargetUnit")
                    .IsRequired()
                    .HasMaxLength(50);

                targetBuilder.Property(t => t.TargetDate)
                    .HasColumnName("TargetDate")
                    .IsRequired();
            });

            // Indexing
            builder.HasIndex(x => x.SustainableGrowthFrameworkId);
            builder.HasIndex(x => x.Category);
            builder.HasIndex(x => x.LastMeasurementDate);
        }
    }
}
