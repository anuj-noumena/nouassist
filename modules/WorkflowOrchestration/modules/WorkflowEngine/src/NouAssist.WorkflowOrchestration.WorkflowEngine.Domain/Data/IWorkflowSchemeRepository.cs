using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Data
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
