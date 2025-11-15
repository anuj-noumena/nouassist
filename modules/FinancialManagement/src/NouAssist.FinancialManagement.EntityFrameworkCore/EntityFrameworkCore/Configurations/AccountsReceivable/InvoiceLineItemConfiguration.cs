using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using NouAssist.FinancialManagement.Shared;
using NouAssist.FinancialManagement.Shared.Constants;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsReceivable
{
    public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "InvoiceLineItems",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.InvoiceId).IsRequired();
            
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxDescriptionLength);

            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasPrecision(18, 4);

            // UnitPrice value object
            builder.OwnsOne(x => x.UnitPrice, up =>
            {
                up.Property(m => m.Amount)
                    .HasColumnName("UnitPrice")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                up.Property(m => m.Currency)
                    .HasColumnName("UnitPriceCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // SubTotal value object
            builder.OwnsOne(x => x.SubTotal, st =>
            {
                st.Property(m => m.Amount)
                    .HasColumnName("SubTotal")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                st.Property(m => m.Currency)
                    .HasColumnName("SubTotalCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.Property(x => x.Notes)
                .HasMaxLength(FinancialConsts.MaxDescriptionLength);

            builder.Property(x => x.ProductId);
            builder.Property(x => x.TaxCode).HasMaxLength(50);

            // Indexes
            builder.HasIndex(x => x.InvoiceId);
            builder.HasIndex(x => x.ProductId);
        }
    }
}
