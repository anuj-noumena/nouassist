using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsPayable.Entities;
using NouAssist.FinancialManagement.Shared.Constants;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.AccountsPayable.Configurations
{
    public class BillApprovalConfiguration : IEntityTypeConfiguration<BillApproval>
    {
        public void Configure(EntityTypeBuilder<BillApproval> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "BillApprovals",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => new { x.BillId, x.ApproverId, x.Level });

            builder.Property(x => x.BillId).IsRequired();
            builder.Property(x => x.ApproverId).IsRequired();
            
            builder.Property(x => x.Level)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ApprovedDate).IsRequired();
            
            builder.Property(x => x.Comments)
                .HasMaxLength(FinancialConsts.MaxDescriptionLength);

            // Indexes
            builder.HasIndex(x => x.BillId);
            builder.HasIndex(x => x.ApproverId);
        }
    }
}
