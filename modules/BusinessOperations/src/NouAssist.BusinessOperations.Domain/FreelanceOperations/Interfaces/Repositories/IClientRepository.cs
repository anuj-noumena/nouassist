using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for FreelanceClient aggregate
    /// </summary>
    public interface IClientRepository : IRepository<FreelanceClient, Guid>
    {
        /// <summary>
        /// Gets list of clients using the given specification
        /// </summary>
        Task<List<FreelanceClient>> GetListAsync(
            Specification<FreelanceClient> specification,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets count of clients using the given specification
        /// </summary>
        Task<int> CountAsync(
            Specification<FreelanceClient> specification,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Checks if any clients match the given specification
        /// </summary>
        Task<bool> AnyAsync(
            Specification<FreelanceClient> specification,
            CancellationToken cancellationToken = default);
    }
}
