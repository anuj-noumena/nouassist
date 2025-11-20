using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.Shared.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.AccountsPayable.Configurations
{
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "Bills",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.BillNumber)
                .IsRequired()
                .HasMaxLength(BillConsts.MaxBillNumberLength);

            builder.Property(x => x.VendorId).IsRequired();
            
            builder.Property(x => x.VendorName)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxNameLength);

            // Amount value object
            builder.OwnsOne(x => x.TotalAmount, a =>
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

            builder.Property(x => x.IssueDate).IsRequired();
            builder.Property(x => x.DueDate).IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.ProjectId);
            builder.Property(x => x.DocumentId);
            builder.Property(x => x.RequiresApproval).IsRequired();

            // Navigation to approvals
            builder.HasMany(x => x.Approvals)
                .WithOne()
                .HasForeignKey(a => a.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            // Navigation to line items
            builder.HasMany(x => x.LineItems)
                .WithOne()
                .HasForeignKey(li => li.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.VendorId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.DueDate);
            builder.HasIndex(x => new { x.VendorId, x.Status, x.DueDate })
                .HasDatabaseName("IX_Bills_Vendor_Status_DueDate");
            
            builder.HasIndex(x => new { x.TenantId, x.VendorId, x.BillNumber })
                .IsUnique()
                .HasDatabaseName("IX_Bills_Tenant_Vendor_Number");

            // Covering index for aging reports
            builder.HasIndex(x => new { x.Status, x.DueDate })
                .HasDatabaseName("IX_Bills_Aging")
                .HasFilter("[Status] IN ('Approved', 'Scheduled', 'Overdue')");

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
