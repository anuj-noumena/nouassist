using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Approval.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Approval
{
    [ConnectionStringName(ApprovalDbProperties.ConnectionStringName)]
    public interface IApprovalDbContext : IEfCoreDbContext
    {
        DbSet<ApprovalWorkflow> ApprovalWorkflows { get; }
        DbSet<ApprovalRequest> ApprovalRequests { get; }
    }
}
