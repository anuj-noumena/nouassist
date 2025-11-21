using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Repositories
{
    /// <summary>
    /// Repository interface for GlossaryTerm aggregate
    /// </summary>
    public interface IGlossaryTermRepository : IRepository<GlossaryTerm, Guid>
    {
        /// <summary>
        /// Finds a glossary term by name
        /// </summary>
        Task<GlossaryTerm?> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a term with the given name already exists
        /// </summary>
        Task<bool> ExistsByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets terms matching the given specification
        /// </summary>
        Task<List<GlossaryTerm>> GetListAsync(
            Specification<GlossaryTerm> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all approved terms
        /// </summary>
        Task<List<GlossaryTerm>> GetApprovedTermsAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets terms pending approval
        /// </summary>
        Task<List<GlossaryTerm>> GetPendingApprovalAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches terms by name (partial match)
        /// </summary>
        Task<List<GlossaryTerm>> SearchByNameAsync(
            string searchTerm,
            int maxResults = 50,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets related terms by IDs
        /// </summary>
        Task<List<GlossaryTerm>> GetRelatedTermsAsync(
            List<Guid> termIds,
            CancellationToken cancellationToken = default);
    }
}
