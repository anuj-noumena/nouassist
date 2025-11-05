using Microsoft.EntityFrameworkCore;
using NouAssist.Approval.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Approval.EntityFrameworkCore
{
    [ConnectionStringName(ApprovalDbProperties.ConnectionStringName)]
    public interface IApprovalDbContext : IEfCoreDbContext
    {
        DbSet<ApprovalWorkflow> ApprovalWorkflows { get; }
        DbSet<ApprovalRequest> ApprovalRequests { get; }
    }
}
