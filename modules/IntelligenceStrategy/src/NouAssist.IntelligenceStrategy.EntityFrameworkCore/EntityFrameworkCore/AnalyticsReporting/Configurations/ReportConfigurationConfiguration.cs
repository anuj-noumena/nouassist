using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Analytics.Aggregates;
using NouAssist.Analytics.Constants;
using NouAssist.Analytics.Entities;
using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Analytics.EntityFrameworkCore.EntityConfigurations
{
    /// <summary>
    /// Entity configuration for ReportConfiguration aggregate
    /// </summary>
    public class ReportConfigurationConfiguration : IEntityTypeConfiguration<ReportConfiguration>
    {
        public void Configure(EntityTypeBuilder<ReportConfiguration> builder)
        {
            builder.ToTable(AnalyticsConsts.ModuleName + "ReportConfigurations");
            
            builder.ConfigureByConvention();
            
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AnalyticsConsts.Report.MaxNameLength);
                
            builder.Property(x => x.Description)
                .HasMaxLength(AnalyticsConsts.Report.MaxDescriptionLength);
                
            builder.Property(x => x.DefaultFormat)
                .IsRequired();
                
            builder.HasIndex(x => new { x.TenantId, x.Name })
                .IsUnique();
            
            // Configure ReportParameter as owned entity collection
            builder.OwnsMany(
                x => x.Parameters,
                parameters =>
                {
                    parameters.WithOwner().HasForeignKey("ReportConfigurationId");
                    parameters.HasKey("ReportConfigurationId", nameof(ReportParameter.Name));
                    
                    parameters.Property(p => p.Name).IsRequired().HasMaxLength(100);
                    parameters.Property(p => p.DisplayName).IsRequired().HasMaxLength(200);
                    parameters.Property(p => p.DataType).IsRequired().HasMaxLength(50);
                    parameters.Property(p => p.ValidationRegex).HasMaxLength(500);
                }
            );
            
            // Configure ReportSchedule as owned entity
            builder.OwnsOne(
                x => x.Schedule,
                schedule =>
                {
                    schedule.Property(s => s.IsEnabled).IsRequired();
                    schedule.Property(s => s.CronExpression).IsRequired().HasMaxLength(100);
                    schedule.Property(s => s.NextRunTime).IsRequired();
                    schedule.Property(s => s.EmailRecipients).HasMaxLength(1000);
                    schedule.Property(s => s.StoreInRepository).IsRequired();
                }
            );
            
            // Configure ReportGenerationLog as owned entity collection
            builder.OwnsMany(
                x => x.GenerationLogs,
                logs =>
                {
                    logs.WithOwner().HasForeignKey("ReportConfigurationId");
                    logs.HasKey(l => l.Id);
                    
                    logs.Property(l => l.GeneratedAt).IsRequired();
                    logs.Property(l => l.IsSuccessful).IsRequired();
                    logs.Property(l => l.Format).IsRequired();
                    logs.Property(l => l.IsScheduled).IsRequired();
                    logs.Property(l => l.ErrorMessage).HasMaxLength(1000);
                    logs.Property(l => l.StoragePath).HasMaxLength(500);
                }
            );
            
            // Configure DataSources as a collection of Guids
            builder.Property(x => x.DataSources)
                .HasColumnName("DataSources")
                .HasColumnType("jsonb"); // For PostgreSQL
        }
    }
}
