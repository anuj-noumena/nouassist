using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.Shared;
using NouAssist.FinancialManagement.Shared.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsReceivable
{
    public class CustomerPaymentConfiguration : IEntityTypeConfiguration<CustomerPayment>
    {
        public void Configure(EntityTypeBuilder<CustomerPayment> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "CustomerPayments",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CustomerId).IsRequired();

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

            builder.Property(x => x.PaymentDate).IsRequired();
            
            builder.Property(x => x.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Reference)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxReferenceLength);

            builder.Property(x => x.PaymentSource)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasIndex(x => x.CustomerId);
            builder.HasIndex(x => x.PaymentDate);
            builder.HasIndex(x => x.Reference);

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
