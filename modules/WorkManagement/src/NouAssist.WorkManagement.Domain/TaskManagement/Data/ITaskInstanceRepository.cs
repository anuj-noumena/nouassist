using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.SharedKernel.Enums;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskInstanceRepository : IRepository<TaskInstance, Guid>
    {
        Task<TaskInstance> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<TaskInstance>> GetListAsync(
            ISpecification<TaskInstance> spec,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<List<TaskInstance>> GetOverdueTasksAsync(
            DateTime asOfDate,
            CancellationToken cancellationToken = default);

        Task<List<TaskInstance>> GetTaskInstancesByProjectAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);

        Task<Dictionary<Guid, int>> GetTaskInstanceCountByStatusAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);

        Task<List<TaskInstance>> GetTasksByAreaAsync(
            Guid areaId,
            CancellationToken cancellationToken = default);

        Task<List<TaskInstance>> GetTasksByContextAsync(
            Guid contextId,
            CancellationToken cancellationToken = default);

        Task<List<TaskInstance>> GetNextActionsAsync(
            Guid? projectId = null,
            Guid? areaId = null,
            Guid? contextId = null,
            CancellationToken cancellationToken = default);

        Task<List<TaskInstance>> GetOverdueTasksFilteredAsync(
            Guid? projectId = null,
            Guid? areaId = null,
            CancellationToken cancellationToken = default);

        Task<List<TaskInstance>> GetFilteredListAsync(
            Guid? projectId = null,
            Guid? areaId = null,
            Guid? contextId = null,
            Guid? assigneeId = null,
            Guid? ownerId = null,
            Guid? workflowStatusId = null,
            bool? isBlocked = null,
            bool? isNextAction = null,
            bool? isOverdue = null,
            EisenhowerQuadrant? quadrant = null,
            DateTime? dueDateFrom = null,
            DateTime? dueDateTo = null,
            CancellationToken cancellationToken = default);
    }
}
