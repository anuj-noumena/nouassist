using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Configurations
{
    public class MetricGoalConfiguration : IEntityTypeConfiguration<MetricGoal>
    {
        public void Configure(EntityTypeBuilder<MetricGoal> builder)
        {
            builder.ToTable(PerformanceFinancialModelingDbProperties.DbTablePrefix + "MetricGoals", 
                           PerformanceFinancialModelingDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.MetricId)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxNotesLength);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.IsAchieved)
                .IsRequired();

            builder.Property(x => x.AchievedAt);

            // Value Object Mapping - Target (GoalTarget)
            builder.OwnsOne(x => x.Target, targetBuilder =>
            {
                targetBuilder.Property(t => t.TargetDate)
                    .HasColumnName("TargetDate")
                    .IsRequired();

                targetBuilder.Property(t => t.ThresholdType)
                    .HasColumnName("ThresholdType")
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

                // Nested Value Object - TargetValue (MetricValue)
                targetBuilder.OwnsOne(t => t.TargetValue, valueBuilder =>
                {
                    valueBuilder.Property(v => v.Value)
                        .HasColumnName("TargetValue")
                        .HasColumnType("decimal(18,2)")
                        .IsRequired();

                    valueBuilder.Property(v => v.DataType)
                        .HasColumnName("TargetDataType")
                        .HasConversion<string>()
                        .HasMaxLength(50)
                        .IsRequired();

                    valueBuilder.Property(v => v.Unit)
                        .HasColumnName("TargetUnit")
                        .HasMaxLength(20);
                });

                // Nested Value Object - SecondaryValue (MetricValue) - for Between/Outside thresholds
                targetBuilder.OwnsOne(t => t.SecondaryValue, valueBuilder =>
                {
                    valueBuilder.Property(v => v.Value)
                        .HasColumnName("SecondaryValue")
                        .HasColumnType("decimal(18,2)");

                    valueBuilder.Property(v => v.DataType)
                        .HasColumnName("SecondaryDataType")
                        .HasConversion<string>()
                        .HasMaxLength(50);

                    valueBuilder.Property(v => v.Unit)
                        .HasColumnName("SecondaryUnit")
                        .HasMaxLength(20);
                });
            });

            // Indexes
            builder.HasIndex(x => x.MetricId);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.IsAchieved);
        }
    }
}
