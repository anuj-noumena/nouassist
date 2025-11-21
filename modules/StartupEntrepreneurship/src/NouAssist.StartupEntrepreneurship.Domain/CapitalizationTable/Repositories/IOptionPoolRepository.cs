using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications.Base;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories
{
    /// <summary>
    /// Repository for OptionPool aggregate
    /// </summary>
    public interface IOptionPoolRepository : IRepository<OptionPool, Guid>
    {
        /// <summary>
        /// Gets option pools by cap table id
        /// </summary>
        Task<List<OptionPool>> GetByCapTableIdAsync(
            Guid capTableId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets active option pools
        /// </summary>
        Task<List<OptionPool>> GetActivePoolsAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds option pool by name
        /// </summary>
        Task<OptionPool> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets option pools with available options
        /// </summary>
        Task<List<OptionPool>> GetPoolsWithAvailableOptionsAsync(
            CancellationToken cancellationToken = default);
    }
}
