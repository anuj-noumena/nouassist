using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Data;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement.Repositories
{
    public class EfCoreProjectRepository
        : EfCoreRepository<IProjectsDbContext, Project, Guid>, IProjectRepository
    {
        public EfCoreProjectRepository(
            IDbContextProvider<IProjectsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Project> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Project>> GetListAsync(
            ISpecification<Project> specification,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query.Where(specification.ToExpression());

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<Project>> GetByOwnerAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(p => p.OwnerId == ownerId && !p.IsArchived)
                .OrderByDescending(p => p.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Project>> GetOverdueProjectsAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            var now = DateTime.UtcNow;

            return await query
                .Where(p => p.Timeline.PlannedDueDate != null &&
                            p.Timeline.PlannedDueDate.Value < now &&
                            p.Status != ProjectStatus.Completed &&
                            p.Status != ProjectStatus.Cancelled &&
                            !p.IsArchived)
                .OrderBy(p => p.Timeline.PlannedDueDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Project>> GetByAreaAsync(
            Guid areaId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(p => p.AreaId == areaId && !p.IsArchived)
                .OrderByDescending(p => p.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Project>> GetActiveProjectsAsync(
            Guid? ownerId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query.Where(p => p.Status == ProjectStatus.Active && !p.IsArchived);

            if (ownerId.HasValue)
            {
                query = query.Where(p => p.OwnerId == ownerId.Value);
            }

            return await query
                .OrderBy(p => p.Timeline.PlannedDueDate)
                .ToListAsync(cancellationToken);
        }

// Note: This one is tricky because Health is a computed property
// You might need to load projects and filter in memory, or create a database view
        public async Task<List<Project>> GetProjectsByHealthAsync(
            ProjectHealth health,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            // Load all non-archived projects first
            var projects = await query
                .Where(p => !p.IsArchived)
                .ToListAsync(cancellationToken);

            // Filter by computed Health property in memory
            return projects
                .Where(p => p.Health == health)
                .ToList();
        }

        public async Task<List<Project>> GetProjectsStartingInRangeAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(p => p.Timeline.PlannedStartDate != null &&
                            p.Timeline.PlannedStartDate.Value >= startDate &&
                            p.Timeline.PlannedStartDate.Value <= endDate &&
                            !p.IsArchived)
                .OrderBy(p => p.Timeline.PlannedStartDate)
                .ToListAsync(cancellationToken);
        }
    }
}