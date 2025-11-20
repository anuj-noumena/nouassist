using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Strategic Goal Definition aggregate
    /// </summary>
    public class StrategicGoalDefinitionConfiguration : IEntityTypeConfiguration<StrategicGoalDefinition>
    {
        public void Configure(EntityTypeBuilder<StrategicGoalDefinition> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "StrategicGoals", 
                            StrategicPlanningDbProperties.DbSchema);

            // Primary Key (handled by AggregateRoot<TKey>)
            builder.HasKey(x => x.Id);

            // Value Object Mapping - GoalDescription
            builder.OwnsOne(x => x.Description, descBuilder => 
            {
                descBuilder.Property(d => d.Title)
                    .HasColumnName("Title")
                    .IsRequired()
                    .HasMaxLength(StrategicPlanningConsts.MaxGoalTitleLength);
                
                descBuilder.Property(d => d.Description)
                    .HasColumnName("Description")
                    .IsRequired()
                    .HasMaxLength(StrategicPlanningConsts.MaxGoalDescriptionLength);
            });

            // Enum Mappings
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Priority)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Horizon)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Property Mappings
            builder.Property(x => x.TargetDate)
                .IsRequired();

            builder.Property(x => x.OwnerId)
                .IsRequired(false);

            builder.Property(x => x.Notes)
                .HasMaxLength(StrategicPlanningConsts.MaxNotesLength);

            // Collection Mapping - Tags as JSON
            builder.Property(x => x.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnType("jsonb");

            // Relationship Mapping - One-to-Many for GoalTargets
            builder.HasMany<GoalTarget>()
                   .WithOne()
                   .HasForeignKey(t => t.StrategicGoalId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Priority);
            builder.HasIndex(x => x.Horizon);
            builder.HasIndex(x => x.TargetDate);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.TenantId);

            // Extra Properties (ABP handles JSON mapping by default)
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(StrategicGoalDefinition.ExtraProperties))
                .HasColumnType("jsonb");

            // Concurrency Stamp (Handled by ConfigureByConvention)
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
