using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using NouAssist.FinancialManagement.Shared;
using NouAssist.FinancialManagement.Shared.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.GeneralLedger
{
    public class JournalEntryBatchConfiguration : IEntityTypeConfiguration<JournalEntryBatch>
    {
        public void Configure(EntityTypeBuilder<JournalEntryBatch> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "JournalEntryBatches",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.BatchNumber)
                .IsRequired()
                .HasMaxLength(50);

            // PostingDate value object
            builder.OwnsOne(x => x.PostingDate, pd =>
            {
                pd.Property(d => d.Date)
                    .HasColumnName("PostingDate")
                    .IsRequired();
            });

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxDescriptionLength);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Reference)
                .HasMaxLength(FinancialConsts.MaxReferenceLength);

            builder.Property(x => x.SourceEntityId);
            
            builder.Property(x => x.SourceEntityType)
                .HasConversion<string>()
                .HasMaxLength(50);

            // Navigation to lines (owned collection)
            builder.HasMany(x => x.Lines)
                .WithOne()
                .HasForeignKey(l => l.BatchId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.BatchNumber);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => new { x.TenantId, x.PostingDate });
            builder.HasIndex(x => new { x.Status, x.PostingDate })
                .HasDatabaseName("IX_JournalBatches_Status_PostingDate");

            // Unique constraint on batch number within tenant
            builder.HasIndex(x => new { x.TenantId, x.BatchNumber })
                .IsUnique()
                .HasDatabaseName("IX_JournalBatches_TenantId_BatchNumber");

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
