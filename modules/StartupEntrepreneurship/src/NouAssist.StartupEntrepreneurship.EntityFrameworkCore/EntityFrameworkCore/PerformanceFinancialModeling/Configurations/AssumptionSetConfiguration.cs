using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Newtonsoft.Json;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Configurations
{
    public class AssumptionSetConfiguration : IEntityTypeConfiguration<AssumptionSet>
    {
        public void Configure(EntityTypeBuilder<AssumptionSet> builder)
        {
            builder.ToTable(PerformanceFinancialModelingDbProperties.DbTablePrefix + "AssumptionSets", 
                           PerformanceFinancialModelingDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ModelId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxAssumptionNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxAssumptionDescriptionLength);

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Assumptions stored as JSON
            builder.Property(x => x.Assumptions)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v, Formatting.None),
                    v => JsonConvert.DeserializeObject<List<AssumptionValue>>(v) ?? new List<AssumptionValue>())
                .HasColumnName("Assumptions")
                .HasColumnType("text");

            // Indexes
            builder.HasIndex(x => x.ModelId);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => new { x.ModelId, x.Name }).IsUnique();
        }
    }
}
