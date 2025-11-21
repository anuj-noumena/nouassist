using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ProjectExecution.Repositories
{
    /// <summary>
    /// Repository for Project aggregate
    /// </summary>
    public interface IProjectRepository : IRepository<Project, Guid>
    {
        /// <summary>
        /// Gets a project with all its child entities
        /// </summary>
        Task<Project> GetWithDetailsAsync(
            Guid id, 
            bool includePhases = true,
            bool includeMilestones = true,
            bool includeMembers = true,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets a list of projects by organization ID
        /// </summary>
        Task<List<Project>> GetListByOrganizationIdAsync(
            Guid organizationId,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets a list of projects by contact ID
        /// </summary>
        Task<List<Project>> GetListByContactIdAsync(
            Guid contactId,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets a list of projects by status
        /// </summary>
        Task<List<Project>> GetListByStatusAsync(
            ProjectStatus status,
            CancellationToken cancellationToken = default);
    }
}
