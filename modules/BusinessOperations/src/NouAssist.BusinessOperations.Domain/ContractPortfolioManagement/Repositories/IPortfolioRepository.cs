using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Enums;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Specifications;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Repositories
{
    /// <summary>
    /// Repository for Portfolio aggregate
    /// </summary>
    public interface IPortfolioRepository : IRepository<PortfolioAggregate, Guid>
    {
        /// <summary>
        /// Gets portfolios matching a specification
        /// </summary>
        Task<List<PortfolioAggregate>> GetListAsync(
            PortfolioSpecification specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets portfolios with eager loaded items
        /// </summary>
        Task<PortfolioAggregate> GetWithItemsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets portfolios with eager loaded risks
        /// </summary>
        Task<PortfolioAggregate> GetWithRisksAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets portfolios with all related entities
        /// </summary>
        Task<PortfolioAggregate> GetWithAllDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets portfolios by name
        /// </summary>
        Task<PortfolioAggregate> GetByNameAsync(
            string name,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets portfolios containing a specific item
        /// </summary>
        Task<List<PortfolioAggregate>> GetContainingItemAsync(
            Guid itemId,
            PortfolioItemType itemType,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);
    }
}
