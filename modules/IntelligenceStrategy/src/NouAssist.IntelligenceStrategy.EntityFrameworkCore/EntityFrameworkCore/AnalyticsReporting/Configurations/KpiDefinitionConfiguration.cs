using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Analytics.Entities;
using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Analytics.EntityFrameworkCore.EntityConfigurations
{
    /// <summary>
    /// Entity configuration for KpiDefinition aggregate
    /// </summary>
    public class KpiDefinitionConfiguration : IEntityTypeConfiguration<KpiDefinition>
    {
        public void Configure(EntityTypeBuilder<KpiDefinition> builder)
        {
            builder.ToTable(AnalyticsConsts.ModuleName + "KpiDefinitions");
            
            builder.ConfigureByConvention();
            
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AnalyticsConsts.Kpi.MaxNameLength);
                
            builder.Property(x => x.Description)
                .HasMaxLength(AnalyticsConsts.Kpi.MaxDescriptionLength);
                
            builder.Property(x => x.CalculationExpression)
                .IsRequired()
                .HasMaxLength(1000);
                
            builder.Property(x => x.CurrentValue)
                .IsRequired()
                .HasPrecision(18, 4);
                
            builder.Property(x => x.Status)
                .IsRequired();
                
            builder.Property(x => x.LastUpdated)
                .IsRequired();
                
            builder.HasIndex(x => new { x.TenantId, x.Name })
                .IsUnique();
                
            builder.HasIndex(x => new { x.TenantId, x.Status });
            
            // Configure KpiThreshold as owned entity collection
            builder.OwnsMany(
                x => x.Thresholds,
                thresholds =>
                {
                    thresholds.WithOwner().HasForeignKey("KpiDefinitionId");
                    thresholds.HasKey(t => t.Id);
                    
                    thresholds.Property(t => t.Name).IsRequired().HasMaxLength(100);
                    thresholds.Property(t => t.ThresholdValue).IsRequired().HasPrecision(18, 4);
                    thresholds.Property(t => t.IsUpperBound).IsRequired();
                    thresholds.Property(t => t.StatusWhenBreached).IsRequired();
                    thresholds.Property(t => t.Severity).IsRequired();
                    thresholds.Property(t => t.NotificationMessage).HasMaxLength(500);
                    
                    // Configure AlertConditionLogic as owned entity
                    thresholds.OwnsOne(
                        t => t.AlertCondition,
                        alertCondition =>
                        {
                            alertCondition.Property(a => a.ConditionExpression).IsRequired().HasMaxLength(500);
                            alertCondition.Property(a => a.ConsecutiveBreaches).IsRequired();
                            alertCondition.Property(a => a.SuppressMinutes).IsRequired();
                            alertCondition.Property(a => a.NotifyOnBackToNormal).IsRequired();
                        }
                    );
                }
            );
            
            // Configure KpiAlertLog as owned entity collection
            builder.OwnsMany(
                x => x.AlertLogs,
                logs =>
                {
                    logs.WithOwner().HasForeignKey("KpiDefinitionId");
                    logs.HasKey(l => l.Id);
                    
                    logs.Property(l => l.KpiValue).IsRequired().HasPrecision(18, 4);
                    logs.Property(l => l.ThresholdValue).IsRequired().HasPrecision(18, 4);
                    logs.Property(l => l.Status).IsRequired();
                    logs.Property(l => l.OccurredAt).IsRequired();
                    
                    logs.HasIndex(l => l.OccurredAt);
                }
            );
            
            // Configure DataSources as a collection of Guids
            builder.Property(x => x.DataSources)
                .HasColumnName("DataSources")
                .HasColumnType("jsonb"); // For PostgreSQL
        }
    }
}
