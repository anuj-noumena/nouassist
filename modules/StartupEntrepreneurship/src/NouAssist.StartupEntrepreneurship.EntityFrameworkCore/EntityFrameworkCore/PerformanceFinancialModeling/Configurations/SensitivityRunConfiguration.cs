using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Configurations
{
    public class SensitivityRunConfiguration : IEntityTypeConfiguration<SensitivityRun>
    {
        public void Configure(EntityTypeBuilder<SensitivityRun> builder)
        {
            builder.ToTable(PerformanceFinancialModelingDbProperties.DbTablePrefix + "SensitivityRuns", 
                           PerformanceFinancialModelingDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ModelId)
                .IsRequired();

            builder.Property(x => x.ScenarioId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxScenarioNameLength);

            builder.Property(x => x.RunDate)
                .IsRequired();

            builder.Property(x => x.VariablesAnalyzed)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Configuration)
                .HasColumnType("text");

            builder.Property(x => x.Results)
                .HasColumnType("text");

            builder.Property(x => x.Notes)
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxNotesLength);

            // Indexes
            builder.HasIndex(x => x.ModelId);
            builder.HasIndex(x => x.ScenarioId);
            builder.HasIndex(x => x.RunDate);
        }
    }
}
