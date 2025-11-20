using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore.Configurations
{
    public class IntellectualPropertyAssetConfiguration : IEntityTypeConfiguration<IntellectualPropertyAsset>
    {
        public void Configure(EntityTypeBuilder<IntellectualPropertyAsset> builder)
        {
            // Apply ABP conventions (audit properties, multi-tenancy, etc.)
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(
                LegalComplianceDbProperties.DbTablePrefix + "IntellectualPropertyAssets",
                LegalComplianceDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxIPAssetNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxIPDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.FilingDate)
                .IsRequired();

            builder.Property(x => x.RegistrationDate);

            builder.Property(x => x.ExpirationDate);

            // Value Object: IPIdentifier
            builder.OwnsOne(x => x.Identifier, identifierBuilder =>
            {
                identifierBuilder.Property(i => i.Value)
                    .HasColumnName("Identifier")
                    .IsRequired()
                    .HasMaxLength(LegalComplianceConsts.MaxIPIdentifierLength);

                identifierBuilder.Property(i => i.Type)
                    .HasColumnName("IdentifierType")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                identifierBuilder.Property(i => i.IssuingAuthority)
                    .HasColumnName("IssuingAuthority")
                    .IsRequired()
                    .HasMaxLength(LegalComplianceConsts.MaxJurisdictionLength);
            });

            // Value Object: Jurisdiction
            builder.OwnsOne(x => x.Jurisdiction, jurisdictionBuilder =>
            {
                jurisdictionBuilder.Property(j => j.Country)
                    .HasColumnName("JurisdictionCountry")
                    .IsRequired()
                    .HasMaxLength(LegalComplianceConsts.MaxJurisdictionLength);

                jurisdictionBuilder.Property(j => j.State)
                    .HasColumnName("JurisdictionState")
                    .HasMaxLength(LegalComplianceConsts.MaxJurisdictionLength);

                jurisdictionBuilder.Property(j => j.Description)
                    .HasColumnName("JurisdictionDescription")
                    .HasMaxLength(LegalComplianceConsts.MaxNotesLength);
            });

            // Value Object: FilingStatus
            builder.OwnsOne(x => x.Status, statusBuilder =>
            {
                statusBuilder.Property(s => s.Status)
                    .HasColumnName("FilingStatus")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                statusBuilder.Property(s => s.StatusDate)
                    .HasColumnName("FilingStatusDate")
                    .IsRequired();

                statusBuilder.Property(s => s.StatusNotes)
                    .HasColumnName("FilingStatusNotes")
                    .HasMaxLength(LegalComplianceConsts.MaxNotesLength);
            });

            // Value Object: IPValuationEstimate
            builder.OwnsOne(x => x.Valuation, valuationBuilder =>
            {
                valuationBuilder.Property(v => v.EstimatedValue)
                    .HasColumnName("ValuationAmount")
                    .HasColumnType("decimal(18,2)");

                valuationBuilder.Property(v => v.Currency)
                    .HasColumnName("ValuationCurrency")
                    .HasMaxLength(3);

                valuationBuilder.Property(v => v.ValuationDate)
                    .HasColumnName("ValuationDate");

                valuationBuilder.Property(v => v.ValuationMethod)
                    .HasColumnName("ValuationMethod")
                    .HasMaxLength(100);

                valuationBuilder.Property(v => v.ValuationNotes)
                    .HasColumnName("ValuationNotes")
                    .HasMaxLength(LegalComplianceConsts.MaxNotesLength);
            });

            // Relationship with FilingDeadline
            builder.HasMany(x => x.FilingDeadlines)
                .WithOne()
                .HasForeignKey(x => x.AssetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => new { x.Type });
            builder.HasIndex("Identifier", "IdentifierType")
                .IsUnique();
        }
    }
}
