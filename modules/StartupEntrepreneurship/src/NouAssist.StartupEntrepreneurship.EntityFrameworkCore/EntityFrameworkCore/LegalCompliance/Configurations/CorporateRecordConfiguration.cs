using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore.Configurations
{
    public class CorporateRecordConfiguration : IEntityTypeConfiguration<CorporateRecord>
    {
        public void Configure(EntityTypeBuilder<CorporateRecord> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                LegalComplianceDbProperties.DbTablePrefix + "CorporateRecords",
                LegalComplianceDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxRecordTitleLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(LegalComplianceConsts.MaxRecordDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.RecordDate)
                .IsRequired();

            builder.Property(x => x.DocumentUrl)
                .HasMaxLength(512);

            builder.Property(x => x.ReferenceNumber)
                .HasMaxLength(50);

            // Value Object: RecordType
            builder.OwnsOne(x => x.RecordType, recordTypeBuilder =>
            {
                recordTypeBuilder.Property(r => r.Type)
                    .HasColumnName("RecordTypeValue")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                recordTypeBuilder.Property(r => r.Description)
                    .HasColumnName("RecordTypeDescription")
                    .HasMaxLength(LegalComplianceConsts.MaxAgreementDescriptionLength);

                recordTypeBuilder.Property(r => r.RequiresSignature)
                    .HasColumnName("RequiresSignature")
                    .IsRequired()
                    .HasDefaultValue(false);

                recordTypeBuilder.Property(r => r.RequiresNotarization)
                    .HasColumnName("RequiresNotarization")
                    .IsRequired()
                    .HasDefaultValue(false);

                recordTypeBuilder.Property(r => r.RequiresFiling)
                    .HasColumnName("RequiresFiling")
                    .IsRequired()
                    .HasDefaultValue(false);
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

            // Indexes
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.RecordDate);
            builder.HasIndex("FilingStatus");
        }
    }
}
