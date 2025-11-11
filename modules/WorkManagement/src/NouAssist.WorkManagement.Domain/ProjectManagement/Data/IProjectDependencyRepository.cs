using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.ProjectManagement.Data
{
    public interface IProjectDependencyRepository : IRepository<ProjectDependency, Guid>
    {
        Task<List<ProjectDependency>> GetByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);

        Task<List<ProjectDependency>> GetBlockingDependenciesAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);

        Task<List<ProjectDependency>> GetOverdueDependenciesAsync(
            CancellationToken cancellationToken = default);
    }
}
