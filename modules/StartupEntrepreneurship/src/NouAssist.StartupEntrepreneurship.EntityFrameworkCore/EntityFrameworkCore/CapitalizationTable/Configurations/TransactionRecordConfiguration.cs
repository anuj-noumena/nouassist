using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for TransactionRecord entity
    /// </summary>
    public class TransactionRecordConfiguration : IEntityTypeConfiguration<TransactionRecord>
    {
        public void Configure(EntityTypeBuilder<TransactionRecord> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CapTableDbProperties.DbTablePrefix + "TransactionRecords", CapTableDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.CapTableId)
                .IsRequired();

            builder.Property(x => x.TransactionType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.TransactionDate)
                .IsRequired();

            builder.Property(x => x.FromHolderId);
            builder.Property(x => x.ToHolderId);
            builder.Property(x => x.SecurityIssuanceId);
            builder.Property(x => x.ShareClassId);

            builder.Property(x => x.Memo)
                .HasMaxLength(CapTableConsts.MaxTransactionMemoLength);

            builder.Property(x => x.ExternalReference)
                .HasMaxLength(100);

            // Value Object Mappings
            builder.OwnsOne(x => x.Quantity, quantityBuilder =>
            {
                quantityBuilder.Property(q => q.Value)
                    .HasColumnName("Quantity")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired();
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

            builder.OwnsOne(x => x.TotalAmount, totalBuilder =>
            {
                totalBuilder.Property(t => t.Value)
                    .HasColumnName("TotalAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                totalBuilder.Property(t => t.Currency)
                    .HasColumnName("TotalCurrency")
                    .HasMaxLength(3)
                    .IsRequired()
                    .HasDefaultValue("USD");
            });

            // Indexing
            builder.HasIndex(x => x.CapTableId);
            builder.HasIndex(x => x.TransactionType);
            builder.HasIndex(x => x.TransactionDate);
            builder.HasIndex(x => x.FromHolderId);
            builder.HasIndex(x => x.ToHolderId);
            builder.HasIndex(x => x.SecurityIssuanceId);

            // Multi-tenancy support
            builder.HasIndex(x => x.TenantId);
        }
    }
}
