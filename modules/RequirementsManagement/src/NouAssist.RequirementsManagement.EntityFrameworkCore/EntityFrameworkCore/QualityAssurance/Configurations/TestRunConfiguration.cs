using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;
using NouAssist.RequirementsManagement.QualityAssurance.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore.Configurations
{
    public class TestRunConfiguration : IEntityTypeConfiguration<TestRun>
    {
        public void Configure(EntityTypeBuilder<TestRun> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(QADbProperties.DbTablePrefix + "TestRuns", QADbProperties.DbSchema);

            // Composite Key
            builder.HasKey(x => new { x.TestCycleId, x.TestCaseId });

            // Property Mappings
            builder.Property(x => x.TestCycleId)
                .IsRequired();

            builder.Property(x => x.TestCaseId)
                .IsRequired();

            builder.Property(x => x.TestCaseTitle)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.ExpectedOutcome)
                .IsRequired()
                .HasMaxLength(QADomainConstants.MaxExpectedOutcomeLength);

            // Value Object Mappings
            builder.OwnsOne(x => x.ActualOutcome, outcomeBuilder =>
            {
                outcomeBuilder.Property(o => o.Description)
                    .HasColumnName("ActualOutcomeDescription")
                    .HasMaxLength(QADomainConstants.MaxActualOutcomeLength);

                outcomeBuilder.Property(o => o.LogData)
                    .HasColumnName("ActualOutcomeLogData")
                    .HasMaxLength(4000);
            });

            // Enum Mapping
            builder.Property(x => x.Result)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ExecutorId)
                .IsRequired(false);

            builder.Property(x => x.ExecutionTime)
                .IsRequired(false);

            builder.Property(x => x.CreationTime)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.TestCycleId);
            builder.HasIndex(x => x.Result);
            builder.HasIndex(x => x.ExecutorId);
        }
    }
}
