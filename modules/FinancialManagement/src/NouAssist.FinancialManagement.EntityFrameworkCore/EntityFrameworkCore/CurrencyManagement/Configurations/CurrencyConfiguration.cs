using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CurrencyManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CurrencyManagement.EntityFrameworkCore.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                CurrencyManagementDbProperties.DbTablePrefix + "Currencies",
                CurrencyManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // CurrencyCode Value Object
            builder.OwnsOne(x => x.Code, code =>
            {
                code.Property(c => c.Code)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("Code")
                    .IsFixedLength();
            });

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName(nameof(Currency.Name));

            builder.Property(x => x.Symbol)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName(nameof(Currency.Symbol));

            builder.Property(x => x.DecimalPlaces)
                .IsRequired()
                .HasDefaultValue(2)
                .HasColumnName(nameof(Currency.DecimalPlaces));

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(Currency.Status));

            builder.Property(x => x.IsBaseCurrency)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName(nameof(Currency.IsBaseCurrency));

            builder.Property(x => x.DisplayOrder)
                .HasDefaultValue(0)
                .HasColumnName(nameof(Currency.DisplayOrder));

            // Indexes
            builder.HasIndex("Code_Code")
                .IsUnique()
                .HasDatabaseName("IX_Currencies_Code");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Currencies_Status");

            builder.HasIndex(x => x.IsBaseCurrency)
                .HasDatabaseName("IX_Currencies_IsBaseCurrency")
                .HasFilter("[IsBaseCurrency] = 1");

            builder.HasIndex(x => x.DisplayOrder)
                .HasDatabaseName("IX_Currencies_DisplayOrder");
        }
    }
}
