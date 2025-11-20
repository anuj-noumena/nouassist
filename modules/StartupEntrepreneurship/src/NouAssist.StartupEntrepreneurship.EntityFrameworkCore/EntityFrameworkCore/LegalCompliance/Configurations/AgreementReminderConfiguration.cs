using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore.Configurations
{
    public class AgreementReminderConfiguration : IEntityTypeConfiguration<AgreementReminder>
    {
        public void Configure(EntityTypeBuilder<AgreementReminder> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                LegalComplianceDbProperties.DbTablePrefix + "AgreementReminders",
                LegalComplianceDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxAgreementTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(LegalComplianceConsts.MaxAgreementDescriptionLength);

            builder.Property(x => x.ReminderDate)
                .IsRequired();

            builder.Property(x => x.DaysBeforeEvent);

            builder.Property(x => x.IsRecurring)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.RecurrencePattern)
                .HasMaxLength(50);

            builder.Property(x => x.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.CompletedDate);

            // Indexes
            builder.HasIndex(x => x.AgreementId);
            builder.HasIndex(x => new { x.ReminderDate, x.IsCompleted });
        }
    }
}
