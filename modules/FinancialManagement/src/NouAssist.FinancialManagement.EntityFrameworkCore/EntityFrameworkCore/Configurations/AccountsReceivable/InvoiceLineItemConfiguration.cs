using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsReceivable
{
    public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "InvoiceLineItems",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => new { x.InvoiceId, x.Description });

            // Properties
            builder.Property(x => x.InvoiceId).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Quantity).IsRequired().HasPrecision(18, 4);

            // MonetaryValue - UnitPrice
            builder.OwnsOne(x => x.UnitPrice, price =>
            {
                price.Property(p => p.Amount)
                    .HasColumnName("UnitPrice_Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                price.Property(p => p.Currency)
                    .HasColumnName("UnitPrice_Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                price.Ignore(p => p.DecimalPlaces);
            });

            // MonetaryValue - Total
            builder.OwnsOne(x => x.Total, total =>
            {
                total.Property(t => t.Amount)
                    .HasColumnName("Total_Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                total.Property(t => t.Currency)
                    .HasColumnName("Total_Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                total.Ignore(t => t.DecimalPlaces);
            });

            builder.Property(x => x.ProductId);
            builder.Property(x => x.ServiceId);

            // Indexes
            builder.HasIndex(x => x.InvoiceId)
                .HasDatabaseName("IX_InvoiceLineItems_InvoiceId");
        }
    }
}
