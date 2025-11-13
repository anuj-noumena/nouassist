using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.GeneralLedger
{
    public class JournalEntryBatchConfiguration : IEntityTypeConfiguration<JournalEntryBatch>
    {
        public void Configure(EntityTypeBuilder<JournalEntryBatch> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "JournalEntryBatches",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.BatchNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.PostingDate)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Reference)
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.SourceEntityId);
            builder.Property(x => x.SourceEntityType)
                .HasMaxLength(100);

            // Relationship: JournalEntryBatch -> JournalEntryLines (One-to-Many)
            builder.HasMany<JournalEntryLine>()
                .WithOne()
                .HasForeignKey(l => l.BatchId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.BatchNumber)
                .IsUnique()
                .HasDatabaseName("IX_JournalEntryBatches_BatchNumber");

            builder.HasIndex(x => x.PostingDate)
                .HasDatabaseName("IX_JournalEntryBatches_PostingDate");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_JournalEntryBatches_Status");

            builder.HasIndex(x => new { x.PostingDate, x.Status })
                .HasDatabaseName("IX_JournalEntryBatches_PostingDate_Status");

            builder.HasIndex(x => x.SourceEntityId)
                .HasDatabaseName("IX_JournalEntryBatches_SourceEntityId");

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_JournalEntryBatches_TenantId");

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(JournalEntryBatch.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
