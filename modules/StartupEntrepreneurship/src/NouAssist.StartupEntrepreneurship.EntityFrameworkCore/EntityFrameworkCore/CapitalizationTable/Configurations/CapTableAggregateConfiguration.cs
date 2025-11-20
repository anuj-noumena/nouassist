using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for CapTable aggregate
    /// </summary>
    public class CapTableAggregateConfiguration : IEntityTypeConfiguration<Aggregates.CapTable>
    {
        public void Configure(EntityTypeBuilder<Aggregates.CapTable> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CapTableDbProperties.DbTablePrefix + "CapTables", CapTableDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.CompanyName)
                .IsRequired()
                .HasMaxLength(CapTableConsts.MaxCompanyNameLength);

            builder.Property(x => x.CompanyIdentifier)
                .HasMaxLength(CapTableConsts.MaxNameLength);

            builder.Property(x => x.AsOfDate)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Value Object Mappings
            builder.OwnsOne(x => x.TotalAuthorizedShares, shareBuilder =>
            {
                shareBuilder.Property(s => s.Value)
                    .HasColumnName("TotalAuthorizedShares")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired();
            });

            builder.OwnsOne(x => x.TotalIssuedShares, shareBuilder =>
            {
                shareBuilder.Property(s => s.Value)
                    .HasColumnName("TotalIssuedShares")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired()
                    .HasDefaultValue(0);
            });

            builder.OwnsOne(x => x.TotalOutstandingShares, shareBuilder =>
            {
                shareBuilder.Property(s => s.Value)
                    .HasColumnName("TotalOutstandingShares")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired()
                    .HasDefaultValue(0);
            });

            // Relationships - One-to-Many for child entities (using shadow FK)
            builder.HasMany<TransactionRecord>()
                   .WithOne()
                   .HasForeignKey("CapTableId")
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<ValuationHistory>()
                   .WithOne()
                   .HasForeignKey("CapTableId")
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.CompanyName);
            builder.HasIndex(x => x.AsOfDate);
            builder.HasIndex(x => x.IsActive);

            // Multi-tenancy support
            builder.HasIndex(x => x.TenantId);
        }
    }
}
