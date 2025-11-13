using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsReceivable
{
    public class CustomerPaymentConfiguration : IEntityTypeConfiguration<CustomerPayment>
    {
        public void Configure(EntityTypeBuilder<CustomerPayment> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "CustomerPayments",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.CustomerId).IsRequired();

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

            // MonetaryValue - UnappliedAmount
            builder.OwnsOne(x => x.UnappliedAmount, unapplied =>
            {
                unapplied.Property(u => u.Amount)
                    .HasColumnName("UnappliedAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                unapplied.Property(u => u.Currency)
                    .HasColumnName("UnappliedCurrency")
                    .HasMaxLength(3)
                    .IsRequired();

                unapplied.Ignore(u => u.DecimalPlaces);
            });

            builder.Property(x => x.PaymentDate).IsRequired();
            builder.Property(x => x.PaymentMethod).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Reference).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Source)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.IsFullyApplied).IsRequired();

            // Indexes
            builder.HasIndex(x => x.CustomerId)
                .HasDatabaseName("IX_CustomerPayments_CustomerId");

            builder.HasIndex(x => x.PaymentDate)
                .HasDatabaseName("IX_CustomerPayments_PaymentDate");

            builder.HasIndex(x => x.Reference)
                .HasDatabaseName("IX_CustomerPayments_Reference");

            builder.HasIndex(x => x.IsFullyApplied)
                .HasDatabaseName("IX_CustomerPayments_IsFullyApplied");

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_CustomerPayments_TenantId");
        }
    }
}
