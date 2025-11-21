using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Configurations
{
    public class ModelScenarioConfiguration : IEntityTypeConfiguration<ModelScenario>
    {
        public void Configure(EntityTypeBuilder<ModelScenario> builder)
        {
            builder.ToTable(PerformanceFinancialModelingDbProperties.DbTablePrefix + "ModelScenarios", 
                           PerformanceFinancialModelingDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ModelId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxScenarioNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxScenarioDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Associated Assumption Sets stored as JSON
            builder.Property(x => x.AssociatedAssumptionSets)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList())
                .HasColumnName("AssociatedAssumptionSets");

            // Indexes
            builder.HasIndex(x => x.ModelId);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => new { x.ModelId, x.Name }).IsUnique();
        }
    }
}
