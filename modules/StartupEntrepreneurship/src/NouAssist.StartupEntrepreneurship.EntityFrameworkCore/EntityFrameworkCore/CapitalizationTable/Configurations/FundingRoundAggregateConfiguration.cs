using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for FundingRound aggregate
    /// </summary>
    public class FundingRoundAggregateConfiguration : IEntityTypeConfiguration<FundingRound>
    {
        public void Configure(EntityTypeBuilder<FundingRound> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CapTableDbProperties.DbTablePrefix + "FundingRounds", CapTableDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.CapTableId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(CapTableConsts.MaxFundingRoundNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(CapTableConsts.MaxDescriptionLength);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.CloseDate);

            builder.Property(x => x.LeadInvestor)
                .HasMaxLength(CapTableConsts.MaxHolderNameLength);

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

            builder.OwnsOne(x => x.TargetAmount, targetBuilder =>
            {
                targetBuilder.Property(t => t.Value)
                    .HasColumnName("TargetAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                targetBuilder.Property(t => t.Currency)
                    .HasColumnName("TargetCurrency")
                    .HasMaxLength(3)
                    .IsRequired()
                    .HasDefaultValue("USD");
            });

            builder.OwnsOne(x => x.RaisedAmount, raisedBuilder =>
            {
                raisedBuilder.Property(r => r.Value)
                    .HasColumnName("RaisedAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired()
                    .HasDefaultValue(0);
                raisedBuilder.Property(r => r.Currency)
                    .HasColumnName("RaisedCurrency")
                    .HasMaxLength(3)
                    .IsRequired()
                    .HasDefaultValue("USD");
            });

            builder.OwnsOne(x => x.SharesIssued, sharesBuilder =>
            {
                sharesBuilder.Property(s => s.Value)
                    .HasColumnName("SharesIssued")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired()
                    .HasDefaultValue(0);
            });

            builder.OwnsOne(x => x.PricePerShare, priceBuilder =>
            {
                priceBuilder.Property(p => p.Value)
                    .HasColumnName("PricePerShare")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired();
                priceBuilder.Property(p => p.Currency)
                    .HasColumnName("PriceCurrency")
                    .HasMaxLength(3)
                    .IsRequired()
                    .HasDefaultValue("USD");
            });

            // Foreign Key to CapTable
            builder.HasOne<Aggregates.CapTable>()
                   .WithMany()
                   .HasForeignKey(x => x.CapTableId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.CapTableId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.StartDate);
            builder.HasIndex(x => x.CloseDate);

            // Multi-tenancy support
            builder.HasIndex(x => x.TenantId);
        }
    }
}
