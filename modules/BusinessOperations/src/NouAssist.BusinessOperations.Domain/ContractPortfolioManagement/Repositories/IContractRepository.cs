using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Specifications;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Repositories
{
    /// <summary>
    /// Repository for Contract aggregate
    /// </summary>
    public interface IContractRepository : IRepository<ContractAggregate, Guid>
    {
        /// <summary>
        /// Gets contracts matching a specification
        /// </summary>
        Task<List<ContractAggregate>> GetListAsync(
            ContractSpecification specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets contracts with eager loaded versions
        /// </summary>
        Task<ContractAggregate> GetWithVersionsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets contracts with eager loaded amendments
        /// </summary>
        Task<ContractAggregate> GetWithAmendmentsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets contracts with all related entities
        /// </summary>
        Task<ContractAggregate> GetWithAllDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets contracts by reference number
        /// </summary>
        Task<ContractAggregate> GetByReferenceNumberAsync(
            string referenceNumber,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets contracts with alerts due for processing
        /// </summary>
        Task<List<ContractAggregate>> GetWithDueAlertsAsync(
            DateTime currentDate,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);
    }
}
