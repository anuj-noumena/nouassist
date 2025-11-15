using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using NouAssist.FinancialManagement.Shared;
using NouAssist.FinancialManagement.Shared.Constants;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.GeneralLedger
{
    public class JournalEntryLineConfiguration : IEntityTypeConfiguration<JournalEntryLine>
    {
        public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "JournalEntryLines",
                FinancialManagementDbProperties.DbSchema);

            // Composite key
            builder.HasKey(x => new { x.BatchId, x.AccountId, x.Memo });

            builder.Property(x => x.BatchId).IsRequired();
            builder.Property(x => x.AccountId).IsRequired();

            // AccountNumber value object (nullable as it's not required)
            builder.OwnsOne(x => x.AccountNumber, an =>
            {
                an.Property(a => a.Number)
                    .HasColumnName("AccountNumber")
                    .HasMaxLength(AccountConsts.MaxAccountNumberLength);
            });

            // DebitAmount value object
            builder.OwnsOne(x => x.DebitAmount, da =>
            {
                da.Property(a => a.Amount)
                    .HasColumnName("DebitAmount")
                    .HasPrecision(18, 4);
                
                da.Property(a => a.Currency)
                    .HasColumnName("DebitCurrency")
                    .HasMaxLength(3);
            });

            // CreditAmount value object
            builder.OwnsOne(x => x.CreditAmount, ca =>
            {
                ca.Property(a => a.Amount)
                    .HasColumnName("CreditAmount")
                    .HasPrecision(18, 4);
                
                ca.Property(a => a.Currency)
                    .HasColumnName("CreditCurrency")
                    .HasMaxLength(3);
            });

            builder.Property(x => x.Memo)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxMemoLength);

            builder.Property(x => x.Reference)
                .HasMaxLength(FinancialConsts.MaxReferenceLength);

            builder.Property(x => x.SourceEntityId);
            
            builder.Property(x => x.SourceEntityType)
                .HasConversion<string>()
                .HasMaxLength(50);

            // Indexes
            builder.HasIndex(x => x.AccountId);
            builder.HasIndex(x => new { x.BatchId, x.AccountId })
                .HasDatabaseName("IX_JournalLines_Batch_Account");
        }
    }
}
