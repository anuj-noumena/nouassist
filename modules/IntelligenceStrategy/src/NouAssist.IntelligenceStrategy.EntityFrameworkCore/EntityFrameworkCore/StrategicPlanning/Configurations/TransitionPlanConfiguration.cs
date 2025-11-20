using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Transition Plan aggregate
    /// </summary>
    public class TransitionPlanConfiguration : IEntityTypeConfiguration<TransitionPlan>
    {
        public void Configure(EntityTypeBuilder<TransitionPlan> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "TransitionPlans", 
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

            // Enum Mapping
            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.PlannedStartDate)
                .IsRequired();

            builder.Property(x => x.EstimatedCompletionDate)
                .IsRequired();

            builder.Property(x => x.CurrentPhase)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.EstimatedCost)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Relationship Mapping - One-to-Many for CapabilityRoadmapItems
            builder.HasMany<CapabilityRoadmapItem>()
                   .WithOne()
                   .HasForeignKey(c => c.TransitionPlanId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Collection Mapping - Milestones as JSON
            builder.Property(x => x.Milestones)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnType("jsonb");

            // Indexing
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.CurrentPhase);
            builder.HasIndex(x => x.PlannedStartDate);
            builder.HasIndex(x => x.EstimatedCompletionDate);
            builder.HasIndex(x => x.TenantId);

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(TransitionPlan.ExtraProperties))
                .HasColumnType("jsonb");

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
