using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore.Configurations
{
    public class AgreementTermsConfiguration : IEntityTypeConfiguration<AgreementTerms>
    {
        public void Configure(EntityTypeBuilder<AgreementTerms> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                LegalComplianceDbProperties.DbTablePrefix + "AgreementTerms",
                LegalComplianceDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxAgreementTitleLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxAgreementDescriptionLength);

            builder.Property(x => x.IsCritical)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.DeadlineDate);

            builder.Property(x => x.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.CompletedDate);

            // Indexes
            builder.HasIndex(x => x.AgreementId);
            builder.HasIndex(x => new { x.DeadlineDate, x.IsCompleted });
        }
    }
}
