using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for FreelanceProject aggregate
    /// </summary>
    public interface IFreelanceProjectRepository : IRepository<FreelanceProject, Guid>
    {
        /// <summary>
        /// Gets projects for a specific client
        /// </summary>
        Task<List<FreelanceProject>> GetProjectsByClientAsync(
            Guid clientId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets list of projects using the given specification
        /// </summary>
        Task<List<FreelanceProject>> GetListAsync(
            Specification<FreelanceProject> specification,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets count of projects using the given specification
        /// </summary>
        Task<int> CountAsync(
            Specification<FreelanceProject> specification,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Checks if any projects match the given specification
        /// </summary>
        Task<bool> AnyAsync(
            Specification<FreelanceProject> specification,
            CancellationToken cancellationToken = default);
    }
}
