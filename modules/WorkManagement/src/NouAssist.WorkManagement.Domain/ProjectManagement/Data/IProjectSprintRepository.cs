using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.ProjectManagement.Data
{
    public interface IProjectSprintRepository : IRepository<ProjectSprint, Guid>
    {
        Task<List<ProjectSprint>> GetByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);

        Task<ProjectSprint> GetActiveSprintAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);
    }
}
