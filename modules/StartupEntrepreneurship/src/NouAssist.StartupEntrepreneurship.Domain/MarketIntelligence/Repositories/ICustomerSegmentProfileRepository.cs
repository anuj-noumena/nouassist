using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories
{
    /// <summary>
    /// Repository interface for CustomerSegmentProfile aggregate
    /// </summary>
    public interface ICustomerSegmentProfileRepository : IRepository<CustomerSegmentProfile, Guid>
    {
        /// <summary>
        /// Finds a customer segment profile by name
        /// </summary>
        Task<CustomerSegmentProfile> FindByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets customer segment profiles by type
        /// </summary>
        Task<List<CustomerSegmentProfile>> GetByTypeAsync(
            SegmentType type,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets active customer segment profiles
        /// </summary>
        Task<List<CustomerSegmentProfile>> GetActiveAsync(bool includeDetails = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets customer segment profiles by demographic criteria
        /// </summary>
        Task<List<CustomerSegmentProfile>> GetByDemographicCriteriaAsync(
            string ageRange = null,
            string income = null,
            string location = null,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets segments by specification
        /// </summary>
        Task<List<CustomerSegmentProfile>> GetListAsync(
            ISpecification<CustomerSegmentProfile> specification,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);
    }
}
