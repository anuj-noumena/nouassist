using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsPayable
{
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "Bills",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.BillNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.VendorId).IsRequired();

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

            builder.Property(x => x.IssueDate).IsRequired();
            builder.Property(x => x.DueDate).IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.ProjectId);
            builder.Property(x => x.AttachedDocumentId);
            builder.Property(x => x.RequiresApproval).IsRequired();
            builder.Property(x => x.Notes).HasMaxLength(1000);

            // Relationships
            builder.HasMany<BillLineItem>()
                .WithOne()
                .HasForeignKey(l => l.BillId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<BillApproval>()
                .WithOne()
                .HasForeignKey(a => a.BillId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => new { x.VendorId, x.BillNumber })
                .IsUnique()
                .HasDatabaseName("IX_Bills_VendorId_BillNumber");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Bills_Status");

            builder.HasIndex(x => x.DueDate)
                .HasDatabaseName("IX_Bills_DueDate");

            builder.HasIndex(x => new { x.Status, x.DueDate })
                .HasDatabaseName("IX_Bills_Status_DueDate");

            builder.HasIndex(x => x.CategoryId)
                .HasDatabaseName("IX_Bills_CategoryId");

            builder.HasIndex(x => x.ProjectId)
                .HasDatabaseName("IX_Bills_ProjectId");

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_Bills_TenantId");
        }
    }
}
