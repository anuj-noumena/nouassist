using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Timeline Event Marker entity
    /// </summary>
    public class TimelineEventMarkerConfiguration : IEntityTypeConfiguration<TimelineEventMarker>
    {
        public void Configure(EntityTypeBuilder<TimelineEventMarker> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "TimelineEventMarkers", 
                            StrategicPlanningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Foreign Key
            builder.Property(x => x.IntegratedHorizonPlanId)
                .IsRequired();

            // Property Mappings
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.EventType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.IsCriticalPath)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.Dependencies)
                .HasMaxLength(1000);

            // Value Object Mapping - TimelineDate
            builder.OwnsOne(x => x.EventDate, dateBuilder => 
            {
                dateBuilder.Property(d => d.Date)
                    .HasColumnName("EventDate")
                    .IsRequired();
                
                dateBuilder.Property(d => d.Horizon)
                    .HasColumnName("EventHorizon")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                dateBuilder.Property(d => d.Description)
                    .HasColumnName("DateDescription")
                    .IsRequired()
                    .HasMaxLength(200);

                dateBuilder.Property(d => d.IsEstimated)
                    .HasColumnName("IsEstimatedDate")
                    .IsRequired()
                    .HasDefaultValue(false);
            });

            // Collection Mapping - Related Goal IDs as JSON
            builder.Property(x => x.RelatedGoalIds)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnType("jsonb");

            // Indexing
            builder.HasIndex(x => x.IntegratedHorizonPlanId);
            builder.HasIndex(x => x.EventType);
            builder.HasIndex(x => x.IsCriticalPath);
        }
    }
}
