using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for ValuationHistory entity
    /// </summary>
    public class ValuationHistoryConfiguration : IEntityTypeConfiguration<ValuationHistory>
    {
        public void Configure(EntityTypeBuilder<ValuationHistory> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CapTableDbProperties.DbTablePrefix + "ValuationHistory", CapTableDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.CapTableId)
                .IsRequired();

            builder.Property(x => x.ValuationDate)
                .IsRequired();

            builder.Property(x => x.Source)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            builder.Property(x => x.FundingRoundId);

            // Value Object Mappings
            builder.OwnsOne(x => x.Valuation, valuationBuilder =>
            {
                valuationBuilder.Property(v => v.PreMoneyValuation)
                    .HasColumnName("PreMoneyValuation")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                valuationBuilder.Property(v => v.PostMoneyValuation)
                    .HasColumnName("PostMoneyValuation")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                valuationBuilder.Property(v => v.Currency)
                    .HasColumnName("ValuationCurrency")
                    .HasMaxLength(3)
                    .IsRequired()
                    .HasDefaultValue("USD");
            });

            // Indexing
            builder.HasIndex(x => x.CapTableId);
            builder.HasIndex(x => x.ValuationDate);
            builder.HasIndex(x => x.FundingRoundId);

            // Multi-tenancy support
            builder.HasIndex(x => x.TenantId);
        }
    }
}
