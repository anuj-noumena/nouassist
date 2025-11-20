using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Goal Target entity
    /// </summary>
    public class GoalTargetConfiguration : IEntityTypeConfiguration<GoalTarget>
    {
        public void Configure(EntityTypeBuilder<GoalTarget> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "GoalTargets", 
                            StrategicPlanningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Foreign Key
            builder.Property(x => x.StrategicGoalId)
                .IsRequired();

            // Value Object Mapping - TargetMetric
            builder.OwnsOne(x => x.Metric, metricBuilder => 
            {
                metricBuilder.Property(m => m.Name)
                    .HasColumnName("MetricName")
                    .IsRequired()
                    .HasMaxLength(StrategicPlanningConsts.MaxMetricNameLength);
                
                metricBuilder.Property(m => m.Value)
                    .HasColumnName("TargetValue")
                    .IsRequired()
                    .HasColumnType("decimal(18,4)");

                metricBuilder.Property(m => m.Unit)
                    .HasColumnName("Unit")
                    .IsRequired()
                    .HasMaxLength(50);

                metricBuilder.Property(m => m.TargetDate)
                    .HasColumnName("TargetDate")
                    .IsRequired();
            });

            // Property Mappings
            builder.Property(x => x.CurrentValue)
                .HasColumnType("decimal(18,4)");

            builder.Property(x => x.IsAchieved)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.AchievedDate)
                .IsRequired(false);

            builder.Property(x => x.Notes)
                .HasMaxLength(StrategicPlanningConsts.MaxNotesLength);

            // Indexing
            builder.HasIndex(x => x.StrategicGoalId);
            builder.HasIndex(x => x.IsAchieved);
        }
    }
}
