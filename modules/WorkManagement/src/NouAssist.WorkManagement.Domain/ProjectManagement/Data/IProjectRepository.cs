using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.ProjectManagement.Data
{
    public interface IProjectRepository : IRepository<Project, Guid>
    {
        
        Task<Project> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<Project>> GetListAsync(
            ISpecification<Project> specification,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<List<Project>> GetByOwnerAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default);

        Task<List<Project>> GetOverdueProjectsAsync(
            CancellationToken cancellationToken = default);

        Task<List<Project>> GetByAreaAsync(
            Guid areaId,
            CancellationToken cancellationToken = default);

        // NEW: Get active projects
        Task<List<Project>> GetActiveProjectsAsync(
            Guid? ownerId = null,
            CancellationToken cancellationToken = default);

        // NEW: Get projects by health status
        Task<List<Project>> GetProjectsByHealthAsync(
            ProjectHealth health,
            CancellationToken cancellationToken = default);
    
        // NEW: Get projects starting in date range
        Task<List<Project>> GetProjectsStartingInRangeAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default);
    }
}
