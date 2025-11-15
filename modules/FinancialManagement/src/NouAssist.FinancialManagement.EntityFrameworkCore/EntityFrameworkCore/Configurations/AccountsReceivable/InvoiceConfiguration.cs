using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using NouAssist.FinancialManagement.Shared;
using NouAssist.FinancialManagement.Shared.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsReceivable
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "Invoices",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(InvoiceConsts.MaxInvoiceNumberLength);

            builder.Property(x => x.CustomerId).IsRequired();
            
            builder.Property(x => x.CustomerName)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxNameLength);

            builder.Property(x => x.IssueDate).IsRequired();
            builder.Property(x => x.DueDate).IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ProjectId);

            // TotalAmount value object
            builder.OwnsOne(x => x.TotalAmount, ta =>
            {
                ta.Property(m => m.Amount)
                    .HasColumnName("TotalAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                ta.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // PaidAmount value object
            builder.OwnsOne(x => x.PaidAmount, pa =>
            {
                pa.Property(m => m.Amount)
                    .HasColumnName("PaidAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                pa.Property(m => m.Currency)
                    .HasColumnName("PaidAmountCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // TaxAmount value object
            builder.OwnsOne(x => x.TaxAmount, ta =>
            {
                ta.Property(m => m.Amount)
                    .HasColumnName("TaxAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                ta.Property(m => m.Currency)
                    .HasColumnName("TaxAmountCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // Navigation to line items
            builder.HasMany(x => x.LineItems)
                .WithOne()
                .HasForeignKey(li => li.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.CustomerId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.DueDate);
            builder.HasIndex(x => new { x.CustomerId, x.Status, x.DueDate })
                .HasDatabaseName("IX_Invoices_Customer_Status_DueDate");
            
            builder.HasIndex(x => new { x.TenantId, x.InvoiceNumber })
                .IsUnique()
                .HasDatabaseName("IX_Invoices_Tenant_Number");

            // Covering index for AR aging
            builder.HasIndex(x => new { x.Status, x.DueDate })
                .HasDatabaseName("IX_Invoices_Aging")
                .HasFilter("[Status] IN ('Sent', 'PartiallyPaid', 'Overdue')");

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
