using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.GeneralLedger
{
    public class BankStatementConfiguration : IEntityTypeConfiguration<BankStatement>
    {
        public void Configure(EntityTypeBuilder<BankStatement> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "BankStatements",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.BankAccountId).IsRequired();
            builder.Property(x => x.StatementDate).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();

            // MonetaryValue - OpeningBalance
            builder.OwnsOne(x => x.OpeningBalance, opening =>
            {
                opening.Property(o => o.Amount)
                    .HasColumnName("OpeningBalance_Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                opening.Property(o => o.Currency)
                    .HasColumnName("OpeningBalance_Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                opening.Ignore(o => o.DecimalPlaces);
            });

            // MonetaryValue - ClosingBalance
            builder.OwnsOne(x => x.ClosingBalance, closing =>
            {
                closing.Property(c => c.Amount)
                    .HasColumnName("ClosingBalance_Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                closing.Property(c => c.Currency)
                    .HasColumnName("ClosingBalance_Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                closing.Ignore(c => c.DecimalPlaces);
            });

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Relationship: BankStatement -> BankStatementLines
            builder.HasMany<BankStatementLine>()
                .WithOne()
                .HasForeignKey(l => l.StatementId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.BankAccountId)
                .HasDatabaseName("IX_BankStatements_BankAccountId");

            builder.HasIndex(x => x.StatementDate)
                .HasDatabaseName("IX_BankStatements_StatementDate");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_BankStatements_Status");
        }
    }
}
