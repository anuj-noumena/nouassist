using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for SecurityIssuance aggregate
    /// </summary>
    public class SecurityIssuanceAggregateConfiguration : IEntityTypeConfiguration<SecurityIssuance>
    {
        public void Configure(EntityTypeBuilder<SecurityIssuance> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CapTableDbProperties.DbTablePrefix + "SecurityIssuances", CapTableDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.CapTableId)
                .IsRequired();

            builder.Property(x => x.SecurityType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(CapTableConsts.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(CapTableConsts.MaxDescriptionLength);

            builder.Property(x => x.IssuanceDate)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.ShareClassId);

            // Value Object Mappings
            builder.OwnsOne(x => x.TotalAuthorized, shareBuilder =>
            {
                shareBuilder.Property(s => s.Value)
                    .HasColumnName("TotalAuthorized")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired();
            });

            builder.OwnsOne(x => x.TotalIssued, shareBuilder =>
            {
                shareBuilder.Property(s => s.Value)
                    .HasColumnName("TotalIssued")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired()
                    .HasDefaultValue(0);
            });

            builder.OwnsOne(x => x.TotalOutstanding, shareBuilder =>
            {
                shareBuilder.Property(s => s.Value)
                    .HasColumnName("TotalOutstanding")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired()
                    .HasDefaultValue(0);
            });

            builder.OwnsOne(x => x.IssuePrice, priceBuilder =>
            {
                priceBuilder.Property(p => p.Value)
                    .HasColumnName("IssuePriceValue")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired();
                priceBuilder.Property(p => p.Currency)
                    .HasColumnName("IssuePriceCurrency")
                    .HasMaxLength(3)
                    .IsRequired()
                    .HasDefaultValue("USD");
            });

            // Option Details (nullable for stock issuances)
            builder.OwnsOne(x => x.OptionDetails, optionBuilder =>
            {
                optionBuilder.Property(o => o.StrikePrice)
                    .HasColumnName("OptionStrikePrice")
                    .HasColumnType("decimal(18,4)");
                optionBuilder.Property(o => o.ExpiryDate)
                    .HasColumnName("OptionExpiryDate");
                optionBuilder.Property(o => o.Currency)
                    .HasColumnName("OptionCurrency")
                    .HasMaxLength(3)
                    .HasDefaultValue("USD");
            });

            // Relationships - One-to-Many for share classes
            builder.HasMany(x => x.ShareClasses)
                   .WithOne()
                   .HasForeignKey("SecurityIssuanceId")
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Foreign Key to CapTable
            builder.HasOne<Aggregates.CapTable>()
                   .WithMany()
                   .HasForeignKey(x => x.CapTableId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.CapTableId);
            builder.HasIndex(x => x.SecurityType);
            builder.HasIndex(x => x.IssuanceDate);
            builder.HasIndex(x => x.IsActive);

            // Multi-tenancy support
            builder.HasIndex(x => x.TenantId);
        }
    }
}
