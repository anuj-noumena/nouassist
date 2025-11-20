using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore.Configurations
{
    public class FilingDeadlineConfiguration : IEntityTypeConfiguration<FilingDeadline>
    {
        public void Configure(EntityTypeBuilder<FilingDeadline> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                LegalComplianceDbProperties.DbTablePrefix + "FilingDeadlines",
                LegalComplianceDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxObligationTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(LegalComplianceConsts.MaxObligationDescriptionLength);

            builder.Property(x => x.Deadline)
                .IsRequired();

            builder.Property(x => x.IsRecurring)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.RecurrencePattern)
                .HasMaxLength(50);

            builder.Property(x => x.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.CompletedDate);

            // Index for querying approaching deadlines
            builder.HasIndex(x => new { x.Deadline, x.IsCompleted });
            builder.HasIndex(x => x.AssetId);
        }
    }
}
