using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.Shared.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.FinancialPlanning.Configurations
{
    public class LinkedBankAccountConfiguration : IEntityTypeConfiguration<LinkedBankAccount>
    {
        public void Configure(EntityTypeBuilder<LinkedBankAccount> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "LinkedBankAccounts",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();
            
            builder.Property(x => x.BankName)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxNameLength);

            builder.Property(x => x.AccountNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.AccountType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // CurrentBalance value object
            builder.OwnsOne(x => x.CurrentBalance, cb =>
            {
                cb.Property(m => m.Amount)
                    .HasColumnName("CurrentBalance")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                cb.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.Property(x => x.LastSyncDate).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            builder.Property(x => x.ConnectionStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.AccountNumber })
                .IsUnique()
                .HasDatabaseName("IX_LinkedBankAccounts_User_Account");

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
