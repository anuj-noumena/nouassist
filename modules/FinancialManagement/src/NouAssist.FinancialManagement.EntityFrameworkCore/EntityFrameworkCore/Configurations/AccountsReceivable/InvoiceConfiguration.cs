using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsReceivable
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "Invoices",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.CustomerId).IsRequired();
            builder.Property(x => x.IssueDate).IsRequired();
            builder.Property(x => x.DueDate).IsRequired();

            // MonetaryValue - TotalAmount
            builder.OwnsOne(x => x.TotalAmount, total =>
            {
                total.Property(t => t.Amount)
                    .HasColumnName("TotalAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                total.Property(t => t.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                total.Ignore(t => t.DecimalPlaces);
            });

            // MonetaryValue - PaidAmount
            builder.OwnsOne(x => x.PaidAmount, paid =>
            {
                paid.Property(p => p.Amount)
                    .HasColumnName("PaidAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                paid.Property(p => p.Currency)
                    .HasColumnName("PaidCurrency")
                    .HasMaxLength(3)
                    .IsRequired();

                paid.Ignore(p => p.DecimalPlaces);
            });

            // MonetaryValue - Balance
            builder.OwnsOne(x => x.Balance, balance =>
            {
                balance.Property(b => b.Amount)
                    .HasColumnName("Balance")
                    .HasPrecision(18, 4)
                    .IsRequired();

                balance.Property(b => b.Currency)
                    .HasColumnName("BalanceCurrency")
                    .HasMaxLength(3)
                    .IsRequired();

                balance.Ignore(b => b.DecimalPlaces);
            });

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ProjectId);
            builder.Property(x => x.Notes).HasMaxLength(1000);

            // Relationships
            builder.HasMany<InvoiceLineItem>()
                .WithOne()
                .HasForeignKey(l => l.InvoiceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<InvoicePaymentApplication>()
                .WithOne()
                .HasForeignKey(a => a.InvoiceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.InvoiceNumber)
                .IsUnique()
                .HasDatabaseName("IX_Invoices_InvoiceNumber");

            builder.HasIndex(x => x.CustomerId)
                .HasDatabaseName("IX_Invoices_CustomerId");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Invoices_Status");

            builder.HasIndex(x => x.DueDate)
                .HasDatabaseName("IX_Invoices_DueDate");

            builder.HasIndex(x => new { x.CustomerId, x.Status })
                .HasDatabaseName("IX_Invoices_CustomerId_Status");

            builder.HasIndex(x => new { x.Status, x.DueDate })
                .HasDatabaseName("IX_Invoices_Status_DueDate")
                .HasFilter("[Status] IN ('Sent', 'PartiallyPaid', 'Overdue')");

            builder.HasIndex(x => x.ProjectId)
                .HasDatabaseName("IX_Invoices_ProjectId");

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_Invoices_TenantId");
        }
    }
}
