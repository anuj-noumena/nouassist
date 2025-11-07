using Microsoft.EntityFrameworkCore;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.EntityFrameworkCore
{
    [ConnectionStringName(ApprovalDbProperties.ConnectionStringName)]
    public interface IApprovalDbContext : IEfCoreDbContext
    {
        DbSet<ApprovalWorkflow> ApprovalWorkflows { get; }
        DbSet<ApprovalRequest> ApprovalRequests { get; }
    }
}
