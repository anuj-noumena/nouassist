using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Proposal aggregate
    /// </summary>
    public interface IProposalRepository : IRepository<Proposal, Guid>
    {
        /// <summary>
        /// Gets proposals for a specific client
        /// </summary>
        Task<List<Proposal>> GetProposalsByClientAsync(
            Guid clientId,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets proposals for a specific project
        /// </summary>
        Task<List<Proposal>> GetProposalsByProjectAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets list of proposals using the given specification
        /// </summary>
        Task<List<Proposal>> GetListAsync(
            Specification<Proposal> specification,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets count of proposals using the given specification
        /// </summary>
        Task<int> CountAsync(
            Specification<Proposal> specification,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Checks if any proposals match the given specification
        /// </summary>
        Task<bool> AnyAsync(
            Specification<Proposal> specification,
            CancellationToken cancellationToken = default);
    }
}
