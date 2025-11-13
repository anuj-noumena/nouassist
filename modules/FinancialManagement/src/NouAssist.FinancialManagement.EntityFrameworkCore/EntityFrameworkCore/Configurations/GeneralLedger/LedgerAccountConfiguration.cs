using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.GeneralLedger
{
    public class LedgerAccountConfiguration : IEntityTypeConfiguration<LedgerAccount>
    {
        public void Configure(EntityTypeBuilder<LedgerAccount> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "LedgerAccounts",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.AccountNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // MonetaryValue - CurrentBalance
            builder.OwnsOne(x => x.CurrentBalance, balance =>
            {
                balance.Property(b => b.Amount)
                    .HasColumnName("CurrentBalance_Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                balance.Property(b => b.Currency)
                    .HasColumnName("CurrentBalance_Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                balance.Ignore(b => b.DecimalPlaces);
            });

            builder.Property(x => x.ParentAccountId);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.AllowsPosting).IsRequired();

            // Indexes
            builder.HasIndex(x => x.AccountNumber)
                .IsUnique()
                .HasDatabaseName("IX_LedgerAccounts_AccountNumber");

            builder.HasIndex(x => x.Type)
                .HasDatabaseName("IX_LedgerAccounts_Type");

            builder.HasIndex(x => x.ParentAccountId)
                .HasDatabaseName("IX_LedgerAccounts_ParentAccountId");

            builder.HasIndex(x => new { x.Type, x.IsActive })
                .HasDatabaseName("IX_LedgerAccounts_Type_IsActive");

            // Multi-tenancy
            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_LedgerAccounts_TenantId");
        }
    }
}
