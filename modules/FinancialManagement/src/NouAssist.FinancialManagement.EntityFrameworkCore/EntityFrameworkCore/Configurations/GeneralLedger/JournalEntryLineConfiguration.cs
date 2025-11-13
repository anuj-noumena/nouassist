using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.GeneralLedger
{
    public class JournalEntryLineConfiguration : IEntityTypeConfiguration<JournalEntryLine>
    {
        public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "JournalEntryLines",
                FinancialManagementDbProperties.DbSchema);

            // Composite Key
            builder.HasKey(x => new { x.BatchId, x.AccountId, x.Memo });

            // Properties
            builder.Property(x => x.BatchId).IsRequired();
            builder.Property(x => x.AccountId).IsRequired();

            // MonetaryValue - DebitAmount
            builder.OwnsOne(x => x.DebitAmount, debit =>
            {
                debit.Property(d => d.Amount)
                    .HasColumnName("DebitAmount")
                    .HasPrecision(18, 4);

                debit.Property(d => d.Currency)
                    .HasColumnName("DebitCurrency")
                    .HasMaxLength(3);

                debit.Ignore(d => d.DecimalPlaces);
            });

            // MonetaryValue - CreditAmount
            builder.OwnsOne(x => x.CreditAmount, credit =>
            {
                credit.Property(c => c.Amount)
                    .HasColumnName("CreditAmount")
                    .HasPrecision(18, 4);

                credit.Property(c => c.Currency)
                    .HasColumnName("CreditCurrency")
                    .HasMaxLength(3);

                credit.Ignore(c => c.DecimalPlaces);
            });

            builder.Property(x => x.Memo)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Reference)
                .HasMaxLength(100);

            builder.Property(x => x.SourceEntityId);
            builder.Property(x => x.SourceEntityType)
                .HasMaxLength(100);

            // Indexes
            builder.HasIndex(x => x.BatchId)
                .HasDatabaseName("IX_JournalEntryLines_BatchId");

            builder.HasIndex(x => x.AccountId)
                .HasDatabaseName("IX_JournalEntryLines_AccountId");

            builder.HasIndex(x => x.SourceEntityId)
                .HasDatabaseName("IX_JournalEntryLines_SourceEntityId");
        }
    }
}
