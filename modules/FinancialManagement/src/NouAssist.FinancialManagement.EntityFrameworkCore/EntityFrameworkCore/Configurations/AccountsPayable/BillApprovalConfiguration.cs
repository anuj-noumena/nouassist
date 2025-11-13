using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsPayable.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsPayable
{
    public class BillApprovalConfiguration : IEntityTypeConfiguration<BillApproval>
    {
        public void Configure(EntityTypeBuilder<BillApproval> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "BillApprovals",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => new { x.BillId, x.ApproverId, x.Level });

            // Properties
            builder.Property(x => x.BillId).IsRequired();
            builder.Property(x => x.ApproverId).IsRequired();

            builder.Property(x => x.Level)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.IsApproved).IsRequired();
            builder.Property(x => x.ApprovedDate).IsRequired();
            builder.Property(x => x.Comments).HasMaxLength(1000);

            // Indexes
            builder.HasIndex(x => x.BillId)
                .HasDatabaseName("IX_BillApprovals_BillId");

            builder.HasIndex(x => x.ApproverId)
                .HasDatabaseName("IX_BillApprovals_ApproverId");
        }
    }
}
