using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.Workflow.Data
{
    public interface IWorkflowRepository : IRepository<Aggregates.Workflow, Guid>
    {
        Task<Aggregates.Workflow> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<Aggregates.Workflow> GetDefaultWorkflowAsync(
            CancellationToken cancellationToken = default);
    }
}
