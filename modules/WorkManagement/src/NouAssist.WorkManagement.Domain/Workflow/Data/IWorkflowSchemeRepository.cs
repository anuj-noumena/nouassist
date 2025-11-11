using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.Workflow.Data
{
    public interface IWorkflowSchemeRepository : IRepository<WorkflowScheme, Guid>
    {
        Task<WorkflowScheme> GetWithMappingsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<WorkflowScheme> GetActiveSchemeForProjectAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);
    }
}
