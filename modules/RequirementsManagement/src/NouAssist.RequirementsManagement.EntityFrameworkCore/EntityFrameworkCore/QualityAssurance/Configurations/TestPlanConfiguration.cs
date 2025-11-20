using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore.Configurations
{
    public class TestPlanConfiguration : IEntityTypeConfiguration<TestPlan>
    {
        public void Configure(EntityTypeBuilder<TestPlan> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(QADbProperties.DbTablePrefix + "TestPlans", QADbProperties.DbSchema);

            // Primary Key (handled by Entity<TKey> / AggregateRoot<TKey>)
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(QADomainConstants.MaxTestPlanNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(QADomainConstants.MaxTestPlanDescriptionLength);

            builder.Property(x => x.ReleaseVersion)
                .IsRequired()
                .HasMaxLength(100);

            // Enum Mapping
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Value Object Mappings
            builder.OwnsOne(x => x.Strategy, strategyBuilder =>
            {
                strategyBuilder.Property(s => s.Approach)
                    .HasColumnName("StrategyApproach")
                    .IsRequired()
                    .HasMaxLength(200);

                strategyBuilder.Property(s => s.CoverageDescription)
                    .HasColumnName("StrategyCoverageDescription")
                    .IsRequired()
                    .HasMaxLength(2000);

                strategyBuilder.Property(s => s.OutOfScope)
                    .HasColumnName("StrategyOutOfScope")
                    .HasMaxLength(2000);
            });

            builder.OwnsOne(x => x.Environment, envBuilder =>
            {
                envBuilder.Property(e => e.Name)
                    .HasColumnName("EnvironmentName")
                    .IsRequired()
                    .HasMaxLength(QADomainConstants.MaxEnvironmentConfigurationLength);

                envBuilder.Property(e => e.Description)
                    .HasColumnName("EnvironmentDescription")
                    .IsRequired()
                    .HasMaxLength(QADomainConstants.MaxEnvironmentConfigurationLength);

                envBuilder.Property(e => e.Url)
                    .HasColumnName("EnvironmentUrl")
                    .HasMaxLength(500);
            });

            // Date properties
            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired();

            // Relationships
            builder.HasMany(x => x.TestCycles)
                .WithOne()
                .HasForeignKey(tc => tc.TestPlanId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Requirement IDs collection mapping (Many-to-Many without navigation property)
            builder.Property(x => x.InScopeRequirementIds)
                .HasColumnName("InScopeRequirementIds")
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => Guid.Parse(id))
                        .ToList());

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.ReleaseVersion);
            builder.HasIndex(x => x.Status);
        }
    }
}
