using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Sustainable Growth Framework aggregate
    /// </summary>
    public class SustainableGrowthFrameworkConfiguration : IEntityTypeConfiguration<SustainableGrowthFramework>
    {
        public void Configure(EntityTypeBuilder<SustainableGrowthFramework> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "SustainableGrowthFrameworks", 
                            StrategicPlanningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(StrategicPlanningConsts.MaxFrameworkNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(StrategicPlanningConsts.MaxEsgGoalDescriptionLength);

            builder.Property(x => x.FrameworkStartDate)
                .IsRequired();

            builder.Property(x => x.FrameworkEndDate)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Relationship Mapping - One-to-Many for EsgGoalRecords
            builder.HasMany<EsgGoalRecord>()
                   .WithOne()
                   .HasForeignKey(e => e.SustainableGrowthFrameworkId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.FrameworkStartDate);
            builder.HasIndex(x => x.FrameworkEndDate);
            builder.HasIndex(x => x.TenantId);

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(SustainableGrowthFramework.ExtraProperties))
                .HasColumnType("jsonb");

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
