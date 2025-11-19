using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.GeneralLedger.Configurations
{
    public class BankStatementConfiguration : IEntityTypeConfiguration<BankStatement>
    {
        public void Configure(EntityTypeBuilder<BankStatement> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "BankStatements",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.BankAccountId).IsRequired();
            builder.Property(x => x.StatementDate).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();

            // OpeningBalance value object
            builder.OwnsOne(x => x.OpeningBalance, ob =>
            {
                ob.Property(b => b.Amount)
                    .HasColumnName("OpeningBalance")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                ob.Property(b => b.Currency)
                    .HasColumnName("OpeningBalanceCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // ClosingBalance value object
            builder.OwnsOne(x => x.ClosingBalance, cb =>
            {
                cb.Property(b => b.Amount)
                    .HasColumnName("ClosingBalance")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                cb.Property(b => b.Currency)
                    .HasColumnName("ClosingBalanceCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Navigation to lines
            builder.HasMany(x => x.Lines)
                .WithOne()
                .HasForeignKey(l => l.StatementId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.BankAccountId);
            builder.HasIndex(x => x.StatementDate);
            builder.HasIndex(x => new { x.BankAccountId, x.StatementDate })
                .HasDatabaseName("IX_BankStatements_Account_Date");

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
