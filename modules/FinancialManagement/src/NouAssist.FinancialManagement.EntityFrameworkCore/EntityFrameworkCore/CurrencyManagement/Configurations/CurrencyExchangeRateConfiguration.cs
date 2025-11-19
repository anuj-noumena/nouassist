using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.CurrencyManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.CurrencyManagement.Configurations
{
    public class CurrencyExchangeRateConfiguration : IEntityTypeConfiguration<CurrencyExchangeRate>
    {
        public void Configure(EntityTypeBuilder<CurrencyExchangeRate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "ExchangeRates",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // BaseCurrency Value Object
            builder.OwnsOne(x => x.BaseCurrency, baseCurrency =>
            {
                baseCurrency.Property(c => c.Code)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("BaseCurrency")
                    .IsFixedLength();
            });

            // TargetCurrency Value Object
            builder.OwnsOne(x => x.TargetCurrency, targetCurrency =>
            {
                targetCurrency.Property(c => c.Code)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("TargetCurrency")
                    .IsFixedLength();
            });

            // ExchangeRate Value Object
            builder.OwnsOne(x => x.Rate, rate =>
            {
                rate.Property(r => r.Rate)
                    .IsRequired()
                    .HasPrecision(18, 6)
                    .HasColumnName("Rate");

                rate.Property(r => r.InverseRate)
                    .IsRequired()
                    .HasPrecision(18, 6)
                    .HasColumnName("InverseRate");

                rate.Property(r => r.EffectiveDate)
                    .IsRequired()
                    .HasColumnName("EffectiveDate");

                rate.Property(r => r.DecimalPlaces)
                    .IsRequired()
                    .HasColumnName("DecimalPlaces");
            });

            builder.Property(x => x.Provider)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(CurrencyExchangeRate.Provider));

            builder.Property(x => x.ProviderReference)
                .HasMaxLength(256)
                .HasColumnName(nameof(CurrencyExchangeRate.ProviderReference));

            builder.Property(x => x.ValidFrom)
                .IsRequired()
                .HasColumnName(nameof(CurrencyExchangeRate.ValidFrom));

            builder.Property(x => x.ValidTo)
                .HasColumnName(nameof(CurrencyExchangeRate.ValidTo));

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName(nameof(CurrencyExchangeRate.IsActive));

            // Indexes for performance
            builder.HasIndex("BaseCurrency_Code", "TargetCurrency_Code", "Rate_EffectiveDate")
                .HasDatabaseName("IX_ExchangeRates_CurrencyPair_Date");

            builder.HasIndex(x => new { x.IsActive, x.ValidFrom, x.ValidTo })
                .HasDatabaseName("IX_ExchangeRates_Active_ValidDates");

            builder.HasIndex("BaseCurrency_Code")
                .HasDatabaseName("IX_ExchangeRates_BaseCurrency");

            builder.HasIndex("TargetCurrency_Code")
                .HasDatabaseName("IX_ExchangeRates_TargetCurrency");

            builder.HasIndex(x => x.Provider)
                .HasDatabaseName("IX_ExchangeRates_Provider");
        }
    }
}
