using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Opportunity Filter Rule entity
    /// </summary>
    public class OpportunityFilterRuleConfiguration : IEntityTypeConfiguration<OpportunityFilterRule>
    {
        public void Configure(EntityTypeBuilder<OpportunityFilterRule> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "OpportunityFilterRules", 
                            StrategicPlanningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Foreign Key
            builder.Property(x => x.OpportunityManagementRecordId)
                .IsRequired();

            // Property Mappings
            builder.Property(x => x.RuleName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.Priority)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            // Value Object Mapping - FilterCriterion
            builder.OwnsOne(x => x.Criterion, criterionBuilder => 
            {
                criterionBuilder.Property(c => c.Name)
                    .HasColumnName("CriterionName")
                    .IsRequired()
                    .HasMaxLength(100);
                
                criterionBuilder.Property(c => c.Description)
                    .HasColumnName("CriterionDescription")
                    .IsRequired()
                    .HasMaxLength(500);

                criterionBuilder.Property(c => c.MinThreshold)
                    .HasColumnName("MinThreshold")
                    .IsRequired()
                    .HasColumnType("decimal(18,4)");

                criterionBuilder.Property(c => c.MaxThreshold)
                    .HasColumnName("MaxThreshold")
                    .IsRequired()
                    .HasColumnType("decimal(18,4)");

                criterionBuilder.Property(c => c.Weight)
                    .HasColumnName("CriterionWeight")
                    .IsRequired();
            });

            // Indexing
            builder.HasIndex(x => x.OpportunityManagementRecordId);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.Priority);
            builder.HasIndex(x => x.CreatedDate);
        }
    }
}
