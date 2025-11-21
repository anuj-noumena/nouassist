using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Newtonsoft.Json;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Configurations
{
    public class MetricDefinitionConfiguration : IEntityTypeConfiguration<MetricDefinition>
    {
        public void Configure(EntityTypeBuilder<MetricDefinition> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(PerformanceFinancialModelingDbProperties.DbTablePrefix + "MetricDefinitions", 
                           PerformanceFinancialModelingDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxDefinitionNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxDefinitionDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.DataType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Unit)
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxMetricUnitLength);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.CategoryId);

            // Value Object Mapping - CalculationRule
            builder.OwnsOne(x => x.CalculationRule, ruleBuilder =>
            {
                ruleBuilder.Property(r => r.Formula)
                    .HasColumnName("CalculationFormula")
                    .HasMaxLength(PerformanceFinancialModelingConsts.MaxCalculationRuleLength)
                    .IsRequired();

                ruleBuilder.Property(r => r.Description)
                    .HasColumnName("CalculationDescription")
                    .HasMaxLength(PerformanceFinancialModelingConsts.MaxDefinitionDescriptionLength)
                    .IsRequired();

                ruleBuilder.Property(r => r.IsAutomated)
                    .HasColumnName("IsAutomatedCalculation")
                    .IsRequired();

                ruleBuilder.Property(r => r.RequiredMetrics)
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v),
                        v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>())
                    .HasColumnName("RequiredMetrics")
                    .HasColumnType("text");
            });

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.CategoryId);
            builder.HasIndex(x => new { x.TenantId, x.Name }).IsUnique();

            // ExtraProperties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(MetricDefinition.ExtraProperties));

            // ConcurrencyStamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
