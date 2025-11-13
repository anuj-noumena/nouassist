using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsReceivable
{
    public class InvoicePaymentApplicationConfiguration : IEntityTypeConfiguration<InvoicePaymentApplication>
    {
        public void Configure(EntityTypeBuilder<InvoicePaymentApplication> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "InvoicePaymentApplications",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => new { x.InvoiceId, x.PaymentId });

            // Properties
            builder.Property(x => x.InvoiceId).IsRequired();
            builder.Property(x => x.PaymentId).IsRequired();

            // MonetaryValue - AppliedAmount
            builder.OwnsOne(x => x.AppliedAmount, applied =>
            {
                applied.Property(a => a.Amount)
                    .HasColumnName("AppliedAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                applied.Property(a => a.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                applied.Ignore(a => a.DecimalPlaces);
            });

            builder.Property(x => x.ApplicationDate).IsRequired();
            builder.Property(x => x.IsReversed).IsRequired();

            // Indexes
            builder.HasIndex(x => x.InvoiceId)
                .HasDatabaseName("IX_InvoicePaymentApplications_InvoiceId");

            builder.HasIndex(x => x.PaymentId)
                .HasDatabaseName("IX_InvoicePaymentApplications_PaymentId");
        }
    }
}
