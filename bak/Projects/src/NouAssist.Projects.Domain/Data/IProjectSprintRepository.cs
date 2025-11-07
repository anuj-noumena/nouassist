using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Projects.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Projects.Data
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
