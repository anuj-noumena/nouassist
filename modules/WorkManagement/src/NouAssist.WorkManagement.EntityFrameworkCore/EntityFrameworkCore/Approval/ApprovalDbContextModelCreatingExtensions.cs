using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.WorkManagement.Approval.EntityFrameworkCore
{
    public static class ApprovalDbContextModelCreatingExtensions
    {
        public static void ConfigureApproval(
            this ModelBuilder builder,
            ApprovalDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes
            builder.ApplyConfigurationsFromAssembly(typeof(ApprovalDbContext).Assembly);
        }
    }
}
