using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Integrated Horizon Plan aggregate
    /// </summary>
    public class IntegratedHorizonPlanConfiguration : IEntityTypeConfiguration<IntegratedHorizonPlan>
    {
        public void Configure(EntityTypeBuilder<IntegratedHorizonPlan> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "IntegratedHorizonPlans", 
                            StrategicPlanningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.PlanName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.PlanStartDate)
                .IsRequired();

            builder.Property(x => x.PlanEndDate)
                .IsRequired();

            // Enum Mapping
            builder.Property(x => x.PrimaryHorizon)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.VisionStatement)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Relationship Mapping - One-to-Many for TimelineEventMarkers
            builder.HasMany<TimelineEventMarker>()
                   .WithOne()
                   .HasForeignKey(t => t.IntegratedHorizonPlanId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Collection Mapping - Future Self Dialogues as JSON
            builder.Property(x => x.FutureSelfDialogues)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnType("jsonb");

            // Indexing
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.PrimaryHorizon);
            builder.HasIndex(x => x.PlanStartDate);
            builder.HasIndex(x => x.PlanEndDate);
            builder.HasIndex(x => x.TenantId);

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(IntegratedHorizonPlan.ExtraProperties))
                .HasColumnType("jsonb");

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
