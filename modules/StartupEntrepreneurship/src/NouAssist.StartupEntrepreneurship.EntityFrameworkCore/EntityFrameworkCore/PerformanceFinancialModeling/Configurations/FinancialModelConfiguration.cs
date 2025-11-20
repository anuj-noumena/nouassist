using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Configurations
{
    public class FinancialModelConfiguration : IEntityTypeConfiguration<FinancialModel>
    {
        public void Configure(EntityTypeBuilder<FinancialModel> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(PerformanceFinancialModelingDbProperties.DbTablePrefix + "FinancialModels", 
                           PerformanceFinancialModelingDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Basic Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxModelNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxModelDescriptionLength);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ProjectionYears)
                .IsRequired();

            builder.Property(x => x.BaseDate)
                .IsRequired();

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(3); // ISO currency code

            builder.Property(x => x.IsActive)
                .IsRequired();

            // One-to-Many: FinancialModel -> ModelScenarios
            builder.HasMany<ModelScenario>()
                .WithOne()
                .HasForeignKey(s => s.ModelId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: FinancialModel -> AssumptionSets
            builder.HasMany<AssumptionSet>()
                .WithOne()
                .HasForeignKey(a => a.ModelId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: FinancialModel -> ProjectionResults
            builder.HasMany<ProjectionResult>()
                .WithOne()
                .HasForeignKey(p => p.ModelId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => new { x.TenantId, x.Name }).IsUnique();

            // ExtraProperties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(FinancialModel.ExtraProperties));

            // ConcurrencyStamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
