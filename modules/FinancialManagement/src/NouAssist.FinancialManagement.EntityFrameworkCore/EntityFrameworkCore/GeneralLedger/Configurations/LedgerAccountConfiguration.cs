using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.Shared.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.GeneralLedger.Configurations
{
    public class LedgerAccountConfiguration : IEntityTypeConfiguration<LedgerAccount>
    {
        public void Configure(EntityTypeBuilder<LedgerAccount> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "LedgerAccounts",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            // AccountNumber value object
            builder.OwnsOne(x => x.AccountNumber, an =>
            {
                an.Property(a => a.Number)
                    .HasColumnName("AccountNumber")
                    .IsRequired()
                    .HasMaxLength(AccountConsts.MaxAccountNumberLength);
            });

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AccountConsts.MaxAccountNameLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // CurrentBalance value object
            builder.OwnsOne(x => x.CurrentBalance, cb =>
            {
                cb.Property(b => b.Amount)
                    .HasColumnName("CurrentBalance")
                    .HasPrecision(18, 4);
                
                cb.Property(b => b.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.Property(x => x.ParentAccountId);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.AllowsPosting).IsRequired();
            
            builder.Property(x => x.Description)
                .HasMaxLength(FinancialConsts.MaxDescriptionLength);

            // Indexes
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.ParentAccountId);
            
            // Unique constraint on account number within tenant
            builder.HasIndex(x => new { x.TenantId, x.AccountNumber })
                .IsUnique()
                .HasDatabaseName("IX_LedgerAccounts_TenantId_AccountNumber");

            // Self-referencing relationship
            builder.HasOne<LedgerAccount>()
                .WithMany()
                .HasForeignKey(x => x.ParentAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // Concurrency token
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
