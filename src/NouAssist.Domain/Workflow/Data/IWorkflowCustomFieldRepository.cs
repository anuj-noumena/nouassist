// src/NouAssist.Workflow.Domain/Data/IWorkflowCustomFieldRepository.cs
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Workflow.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Workflow.Data
{
    public interface IWorkflowCustomFieldRepository : IRepository<WorkflowCustomField, Guid>
    {
        Task<List<WorkflowCustomField>> GetByWorkflowSchemeIdAsync(
            Guid workflowSchemeId,
            CancellationToken cancellationToken = default);

        Task<List<WorkflowCustomField>> GetVisibleInStatusAsync(
            Guid workflowSchemeId,
            Guid statusId,
            CancellationToken cancellationToken = default);
    }
}