using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Analytics.Aggregates;
using NouAssist.Analytics.Constants;
using NouAssist.Analytics.Entities;
using System;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Analytics.EntityFrameworkCore.EntityConfigurations
{
    /// <summary>
    /// Entity configuration for Dashboard aggregate
    /// </summary>
    public class DashboardConfiguration : IEntityTypeConfiguration<Dashboard>
    {
        public void Configure(EntityTypeBuilder<Dashboard> builder)
        {
            builder.ToTable(AnalyticsConsts.ModuleName + "Dashboards");
            
            builder.ConfigureByConvention();
            
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AnalyticsConsts.Dashboard.MaxNameLength);
                
            builder.Property(x => x.Description)
                .HasMaxLength(AnalyticsConsts.Dashboard.MaxDescriptionLength);
                
            builder.Property(x => x.IsDefault)
                .IsRequired();
                
            builder.HasIndex(x => new { x.TenantId, x.Name })
                .IsUnique();
                
            builder.HasIndex(x => new { x.TenantId, x.IsDefault });
            
            // Configure WidgetInstance as owned entity collection
            builder.OwnsMany(
                x => x.Widgets,
                widgets =>
                {
                    widgets.WithOwner().HasForeignKey("DashboardId");
                    widgets.HasKey(w => w.Id);
                    
                    widgets.Property(w => w.Title).IsRequired();
                    widgets.Property(w => w.Type).IsRequired();
                    
                    // Configure the WidgetSettings value object
                    widgets.OwnsOne(
                        w => w.Settings,
                        settings =>
                        {
                            settings.Property(s => s.DataQuery).IsRequired();
                            
                            // Store AdditionalSettings as JSON
                            settings.Property(s => s.AdditionalSettings)
                                .HasColumnName("AdditionalSettings")
                                .HasColumnType("jsonb"); // For PostgreSQL
                        }
                    );
                }
            );
            
            // Configure DashboardLayoutSetting as owned entity
            builder.OwnsOne(
                x => x.LayoutSetting,
                layout =>
                {
                    layout.Property(l => l.Columns).IsRequired();
                    layout.Property(l => l.Theme).IsRequired();
                    layout.Property(l => l.AutoRefresh).IsRequired();
                    layout.Property(l => l.RefreshIntervalSeconds).IsRequired();
                    layout.Property(l => l.CustomCss).HasMaxLength(2000);
                    layout.Property(l => l.CompactView).IsRequired();
                }
            );
        }
    }
}
