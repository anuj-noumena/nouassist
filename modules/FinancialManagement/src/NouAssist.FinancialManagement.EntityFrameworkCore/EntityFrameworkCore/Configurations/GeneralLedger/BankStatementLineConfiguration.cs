using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using NouAssist.FinancialManagement.Shared;
using NouAssist.FinancialManagement.Shared.Constants;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.GeneralLedger
{
    public class BankStatementLineConfiguration : IEntityTypeConfiguration<BankStatementLine>
    {
        public void Configure(EntityTypeBuilder<BankStatementLine> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "BankStatementLines",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.StatementId).IsRequired();
            builder.Property(x => x.TransactionDate).IsRequired();
            
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxDescriptionLength);

            // Amount value object
            builder.OwnsOne(x => x.Amount, a =>
            {
                a.Property(m => m.Amount)
                    .HasColumnName("Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                a.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.Property(x => x.ReferenceNumber)
                .HasMaxLength(FinancialConsts.MaxReferenceLength);

            builder.Property(x => x.IsMatched).IsRequired();
            builder.Property(x => x.MatchedJournalBatchId);
            builder.Property(x => x.MatchedBy);
            builder.Property(x => x.MatchedDate);

            // Indexes
            builder.HasIndex(x => x.StatementId);
            builder.HasIndex(x => x.IsMatched);
            builder.HasIndex(x => x.TransactionDate);
        }
    }
}
