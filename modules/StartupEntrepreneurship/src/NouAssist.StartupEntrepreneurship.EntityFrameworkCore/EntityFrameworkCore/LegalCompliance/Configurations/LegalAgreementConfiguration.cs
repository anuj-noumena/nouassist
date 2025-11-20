using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore.Configurations
{
    public class LegalAgreementConfiguration : IEntityTypeConfiguration<LegalAgreement>
    {
        public void Configure(EntityTypeBuilder<LegalAgreement> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                LegalComplianceDbProperties.DbTablePrefix + "LegalAgreements",
                LegalComplianceDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxAgreementTitleLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxAgreementDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.CounterpartyName)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxPartyNameLength);

            builder.Property(x => x.EffectiveDate)
                .IsRequired();

            builder.Property(x => x.ExpirationDate);

            builder.Property(x => x.ReferenceNumber)
                .HasMaxLength(50);

            builder.Property(x => x.DocumentUrl)
                .HasMaxLength(512);

            builder.Property(x => x.Value)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Currency)
                .HasMaxLength(3);

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

            // Relationships
            builder.HasMany(x => x.Terms)
                .WithOne()
                .HasForeignKey(x => x.AgreementId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Reminders)
                .WithOne()
                .HasForeignKey(x => x.AgreementId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.CounterpartyName);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.EffectiveDate);
            builder.HasIndex(x => x.ExpirationDate);
            builder.HasIndex("FilingStatus");
        }
    }
}
