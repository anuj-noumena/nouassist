using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IProjectRepository
    /// </summary>
    public class EfCoreProjectRepository : 
        EfCoreRepository<IProjectExecutionDbContext, Project, Guid>, 
        IProjectRepository
    {
        public EfCoreProjectRepository(
            IDbContextProvider<IProjectExecutionDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
        
        public async Task<Project> GetWithDetailsAsync(
            Guid id,
            bool includePhases = true,
            bool includeMilestones = true,
            bool includeMembers = true,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // Include requested navigation properties
            if (includePhases)
            {
                query = query.Include(p => p.Phases);
            }
            
            if (includeMilestones)
            {
                query = query.Include(p => p.Milestones);
            }
            
            if (includeMembers)
            {
                query = query.Include(p => p.Members);
            }
            
            return await query
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
        
        public async Task<List<Project>> GetListByOrganizationIdAsync(
            Guid organizationId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(p => p.OrganizationId == organizationId)
                .OrderBy(p => p.Details.StartDate)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Project>> GetListByContactIdAsync(
            Guid contactId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(p => p.ContactId == contactId)
                .OrderBy(p => p.Details.StartDate)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Project>> GetListByStatusAsync(
            ProjectStatus status,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(p => p.Status == status)
                .OrderBy(p => p.Details.StartDate)
                .ToListAsync(cancellationToken);
        }
    }
}
