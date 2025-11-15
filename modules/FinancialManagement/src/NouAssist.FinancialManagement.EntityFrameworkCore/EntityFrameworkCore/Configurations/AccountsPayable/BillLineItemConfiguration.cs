using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsPayable.Entities;
using NouAssist.FinancialManagement.Shared;
using NouAssist.FinancialManagement.Shared.Constants;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsPayable
{
    public class BillLineItemConfiguration : IEntityTypeConfiguration<BillLineItem>
    {
        public void Configure(EntityTypeBuilder<BillLineItem> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "BillLineItems",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => new { x.BillId, x.Description });

            builder.Property(x => x.BillId).IsRequired();
            
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

            // LineTotal value object
            builder.OwnsOne(x => x.LineTotal, lt =>
            {
                lt.Property(m => m.Amount)
                    .HasColumnName("LineTotal")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                lt.Property(m => m.Currency)
                    .HasColumnName("LineTotalCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.Property(x => x.ProductId);
            builder.Property(x => x.TaxCode).HasMaxLength(50);

            // Indexes
            builder.HasIndex(x => x.BillId);
            builder.HasIndex(x => x.ProductId);
        }
    }
}
