using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Newtonsoft.Json;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Configurations
{
    public class ProjectionResultConfiguration : IEntityTypeConfiguration<ProjectionResult>
    {
        public void Configure(EntityTypeBuilder<ProjectionResult> builder)
        {
            builder.ToTable(PerformanceFinancialModelingDbProperties.DbTablePrefix + "ProjectionResults", 
                           PerformanceFinancialModelingDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ModelId)
                .IsRequired();

            builder.Property(x => x.ScenarioId)
                .IsRequired();

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.CalculatedAt)
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxNotesLength);

            // Projected Amounts stored as JSON
            builder.Property(x => x.ProjectedAmounts)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v, Formatting.None),
                    v => JsonConvert.DeserializeObject<List<ProjectedAmount>>(v) ?? new List<ProjectedAmount>())
                .HasColumnName("ProjectedAmounts")
                .HasColumnType("text");

            // Indexes
            builder.HasIndex(x => x.ModelId);
            builder.HasIndex(x => x.ScenarioId);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.CalculatedAt);
            builder.HasIndex(x => new { x.ModelId, x.ScenarioId, x.Type }).IsUnique();
        }
    }
}
