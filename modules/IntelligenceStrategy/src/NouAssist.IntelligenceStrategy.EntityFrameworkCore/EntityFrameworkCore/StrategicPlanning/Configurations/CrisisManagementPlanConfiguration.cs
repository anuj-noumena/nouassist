using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Crisis Management Plan aggregate
    /// </summary>
    public class CrisisManagementPlanConfiguration : IEntityTypeConfiguration<CrisisManagementPlan>
    {
        public void Configure(EntityTypeBuilder<CrisisManagementPlan> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "CrisisManagementPlans", 
                            StrategicPlanningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(StrategicPlanningConsts.MaxPlanNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(StrategicPlanningConsts.MaxContingencyDescriptionLength);

            // Enum Mapping
            builder.Property(x => x.MaxCrisisLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Boolean and DateTime Properties
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.ActivatedAt)
                .IsRequired(false);

            builder.Property(x => x.ActivatedBy)
                .IsRequired(false);

            builder.Property(x => x.LastReviewDate)
                .IsRequired(false);

            builder.Property(x => x.NextReviewDate)
                .IsRequired();

            // Relationship Mapping - One-to-Many for ContingencyPlanItems
            builder.HasMany<ContingencyPlanItem>()
                   .WithOne()
                   .HasForeignKey(c => c.CrisisManagementPlanId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Relationship Mapping - One-to-Many for CrisisProtocolSteps
            builder.HasMany<CrisisProtocolStep>()
                   .WithOne()
                   .HasForeignKey(c => c.CrisisManagementPlanId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.MaxCrisisLevel);
            builder.HasIndex(x => x.NextReviewDate);
            builder.HasIndex(x => x.TenantId);

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(CrisisManagementPlan.ExtraProperties))
                .HasColumnType("jsonb");

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
