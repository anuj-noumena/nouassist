using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsReceivable
{
    public class CreditMemoConfiguration : IEntityTypeConfiguration<CreditMemo>
    {
        public void Configure(EntityTypeBuilder<CreditMemo> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "CreditMemos",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreditMemoNumber).IsRequired().HasMaxLength(50);
            builder.Property(x => x.CustomerId).IsRequired();

            builder.OwnsOne(x => x.Amount, amount =>
            {
                amount.Property(a => a.Amount).HasColumnName("Amount").HasPrecision(18, 4).IsRequired();
                amount.Property(a => a.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
                amount.Ignore(a => a.DecimalPlaces);
            });

            builder.Property(x => x.IssueDate).IsRequired();
            builder.Property(x => x.Reason).IsRequired().HasConversion<string>().HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.OriginalInvoiceId);
            builder.Property(x => x.IsApplied).IsRequired();

            builder.HasIndex(x => x.CreditMemoNumber).IsUnique().HasDatabaseName("IX_CreditMemos_CreditMemoNumber");
            builder.HasIndex(x => x.CustomerId).HasDatabaseName("IX_CreditMemos_CustomerId");
            builder.HasIndex(x => x.OriginalInvoiceId).HasDatabaseName("IX_CreditMemos_OriginalInvoiceId");
            builder.HasIndex(x => x.TenantId).HasDatabaseName("IX_CreditMemos_TenantId");
        }
    }
}
