using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories
{
    /// <summary>
    /// Repository interface for SwotAnalysis aggregate
    /// </summary>
    public interface ISwotAnalysisRepository : IRepository<SwotAnalysis, Guid>
    {
        /// <summary>
        /// Gets completed SWOT analyses
        /// </summary>
        Task<List<SwotAnalysis>> GetCompletedAsync(bool includeDetails = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets incomplete SWOT analyses
        /// </summary>
        Task<List<SwotAnalysis>> GetIncompleteAsync(bool includeDetails = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds a SWOT analysis by title
        /// </summary>
        Task<SwotAnalysis> FindByTitleAsync(string title, bool includeDetails = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets recent SWOT analyses
        /// </summary>
        Task<List<SwotAnalysis>> GetRecentAsync(
            int maxResultCount = 10,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets analyses by specification
        /// </summary>
        Task<List<SwotAnalysis>> GetListAsync(
            ISpecification<SwotAnalysis> specification,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);
    }
}
