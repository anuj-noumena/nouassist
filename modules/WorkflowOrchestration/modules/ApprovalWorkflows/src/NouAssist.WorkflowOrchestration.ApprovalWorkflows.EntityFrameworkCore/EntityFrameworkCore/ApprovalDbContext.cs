using Microsoft.EntityFrameworkCore;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.EntityFrameworkCore
{
    [ConnectionStringName(ApprovalDbProperties.ConnectionStringName)]
    public class ApprovalDbContext : AbpDbContext<ApprovalDbContext>, IApprovalDbContext
    {
        public DbSet<ApprovalWorkflow> ApprovalWorkflows { get; set; }
        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }

        public ApprovalDbContext(DbContextOptions<ApprovalDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureApproval(this);
        }
    }
}
