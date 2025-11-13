using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.GeneralLedger
{
    public class BankStatementLineConfiguration : IEntityTypeConfiguration<BankStatementLine>
    {
        public void Configure(EntityTypeBuilder<BankStatementLine> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "BankStatementLines",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.StatementId).IsRequired();
            builder.Property(x => x.TransactionDate).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.ReferenceNumber).HasMaxLength(100);

            // MonetaryValue - Amount
            builder.OwnsOne(x => x.Amount, amount =>
            {
                amount.Property(a => a.Amount)
                    .HasColumnName("Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                amount.Property(a => a.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                amount.Ignore(a => a.DecimalPlaces);
            });

            builder.Property(x => x.MatchStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Indexes
            builder.HasIndex(x => x.StatementId)
                .HasDatabaseName("IX_BankStatementLines_StatementId");

            builder.HasIndex(x => x.TransactionDate)
                .HasDatabaseName("IX_BankStatementLines_TransactionDate");

            builder.HasIndex(x => x.MatchStatus)
                .HasDatabaseName("IX_BankStatementLines_MatchStatus");
        }
    }
}
