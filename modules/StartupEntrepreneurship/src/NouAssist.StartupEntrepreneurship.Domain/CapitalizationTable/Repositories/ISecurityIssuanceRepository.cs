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
    /// Repository for SecurityIssuance aggregate
    /// </summary>
    public interface ISecurityIssuanceRepository : IRepository<SecurityIssuance, Guid>
    {
        /// <summary>
        /// Finds security issuances matching the given specification
        /// </summary>
        Task<List<SecurityIssuance>> GetListAsync(
            CapTableSpecification<SecurityIssuance> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets security issuances by cap table id
        /// </summary>
        Task<List<SecurityIssuance>> GetByCapTableIdAsync(
            Guid capTableId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets security issuances by security type
        /// </summary>
        Task<List<SecurityIssuance>> GetBySecurityTypeAsync(
            SecurityType securityType,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets active security issuances with available shares
        /// </summary>
        Task<List<SecurityIssuance>> GetActiveWithAvailableSharesAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets expired options as of a specific date
        /// </summary>
        Task<List<SecurityIssuance>> GetExpiredOptionsAsync(
            DateTime asOfDate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets security issuance with share classes
        /// </summary>
        Task<SecurityIssuance> GetWithShareClassesAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
