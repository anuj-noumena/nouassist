using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore.Configurations
{
    public class TestCycleConfiguration : IEntityTypeConfiguration<TestCycle>
    {
        public void Configure(EntityTypeBuilder<TestCycle> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(QADbProperties.DbTablePrefix + "TestCycles", QADbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.TestPlanId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(QADomainConstants.MaxTestCycleNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(QADomainConstants.MaxTestCycleDescriptionLength);

            builder.Property(x => x.BuildInformation)
                .IsRequired()
                .HasMaxLength(QADomainConstants.MaxBuildInformationLength);

            // Enum Mapping
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Date and time properties
            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired();

            builder.Property(x => x.AssigneeId)
                .IsRequired();

            builder.Property(x => x.ActualStartTime)
                .IsRequired(false);

            builder.Property(x => x.ActualEndTime)
                .IsRequired(false);

            // Relationships
            builder.HasMany(x => x.TestRuns)
                .WithOne()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.TestPlanId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.AssigneeId);
        }
    }
}
