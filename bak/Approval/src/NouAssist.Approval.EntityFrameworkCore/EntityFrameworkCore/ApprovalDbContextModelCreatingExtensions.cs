using Microsoft.EntityFrameworkCore;
using NouAssist.Approval.EntityFrameworkCore.Configurations;
using Volo.Abp;

namespace NouAssist.Approval.EntityFrameworkCore
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
