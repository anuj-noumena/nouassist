using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore.Configurations
{
    public class ComplianceObligationConfiguration : IEntityTypeConfiguration<ComplianceObligation>
    {
        public void Configure(EntityTypeBuilder<ComplianceObligation> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                LegalComplianceDbProperties.DbTablePrefix + "ComplianceObligations",
                LegalComplianceDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxObligationTitleLength);

            builder.Property(x => x.AuthorityName)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxPartyNameLength);

            builder.Property(x => x.ReferenceNumber)
                .HasMaxLength(50);

            builder.Property(x => x.Deadline);

            builder.Property(x => x.DocumentUrl)
                .HasMaxLength(512);

            builder.Property(x => x.IsRecurring)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.RecurrencePattern)
                .HasMaxLength(50);

            // Value Object: ObligationDescription
            builder.OwnsOne(x => x.Description, descriptionBuilder =>
            {
                descriptionBuilder.Property(d => d.Overview)
                    .HasColumnName("Overview")
                    .IsRequired()
                    .HasMaxLength(LegalComplianceConsts.MaxObligationDescriptionLength);

                descriptionBuilder.Property(d => d.Requirements)
                    .HasColumnName("Requirements")
                    .IsRequired()
                    .HasMaxLength(LegalComplianceConsts.MaxObligationDescriptionLength);

                descriptionBuilder.Property(d => d.Notes)
                    .HasColumnName("Notes")
                    .HasMaxLength(LegalComplianceConsts.MaxNotesLength);
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

            // Value Object: StatusValue
            builder.OwnsOne(x => x.Status, statusBuilder =>
            {
                statusBuilder.Property(s => s.Status)
                    .HasColumnName("ComplianceStatus")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                statusBuilder.Property(s => s.StatusDate)
                    .HasColumnName("StatusDate")
                    .IsRequired();

                statusBuilder.Property(s => s.Notes)
                    .HasColumnName("StatusNotes")
                    .HasMaxLength(LegalComplianceConsts.MaxNotesLength);
            });

            // Indexes
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.Deadline);
            builder.HasIndex("ComplianceStatus");
            builder.HasIndex("JurisdictionCountry", "JurisdictionState");
        }
    }
}
