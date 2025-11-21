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
    /// Repository for CapTable aggregate
    /// </summary>
    public interface ICapTableRepository : IRepository<Aggregates.CapTable, Guid>
    {
        /// <summary>
        /// Finds cap tables matching the given specification
        /// </summary>
        Task<List<Aggregates.CapTable>> GetListAsync(
            CapTableSpecification<Aggregates.CapTable> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds a cap table by company name
        /// </summary>
        Task<Aggregates.CapTable> FindByCompanyNameAsync(
            string companyName,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets cap table with its transaction history
        /// </summary>
        Task<Aggregates.CapTable> GetWithTransactionsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets cap table with valuation history
        /// </summary>
        Task<Aggregates.CapTable> GetWithValuationHistoryAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts cap tables matching the given specification
        /// </summary>
        Task<int> CountAsync(
            CapTableSpecification<Aggregates.CapTable> specification,
            CancellationToken cancellationToken = default);
    }
}
