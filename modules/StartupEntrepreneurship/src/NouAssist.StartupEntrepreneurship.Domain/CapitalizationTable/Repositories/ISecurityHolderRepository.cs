using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications.Base;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories
{
    /// <summary>
    /// Repository for SecurityHolder aggregate
    /// </summary>
    public interface ISecurityHolderRepository : IRepository<SecurityHolder, Guid>
    {
        /// <summary>
        /// Finds security holders matching the given specification
        /// </summary>
        Task<List<SecurityHolder>> GetListAsync(
            CapTableSpecification<SecurityHolder> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds security holders by holder type
        /// </summary>
        Task<List<SecurityHolder>> GetByHolderTypeAsync(
            HolderType holderType,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds a security holder by name
        /// </summary>
        Task<SecurityHolder> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets security holder with vesting schedules
        /// </summary>
        Task<SecurityHolder> GetWithVestingSchedulesAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a security holder with the given name exists
        /// </summary>
        Task<bool> ExistsByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets security holders by email
        /// </summary>
        Task<SecurityHolder> FindByEmailAsync(
            string email,
            CancellationToken cancellationToken = default);
    }
}
