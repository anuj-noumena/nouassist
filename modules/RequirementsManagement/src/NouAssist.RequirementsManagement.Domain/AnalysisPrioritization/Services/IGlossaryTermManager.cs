using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Services
{
    /// <summary>
    /// Domain service interface for managing glossary terms
    /// </summary>
    public interface IGlossaryTermManager
    {
        Task<GlossaryTerm> CreateAsync(
            string name,
            string definition,
            string? context = null,
            string? examples = null,
            CancellationToken cancellationToken = default);

        Task<GlossaryTerm> UpdateDefinitionAsync(
            Guid termId,
            string newDefinition,
            CancellationToken cancellationToken = default);

        Task ApproveAsync(
            Guid termId,
            Guid approvedBy,
            DateTime approvedAt,
            CancellationToken cancellationToken = default);

        Task RevokeApprovalAsync(
            Guid termId,
            CancellationToken cancellationToken = default);

        Task SetRelatedTermsAsync(
            Guid termId,
            List<Guid> relatedTermIds,
            CancellationToken cancellationToken = default);

        Task<bool> CanDeleteAsync(
            Guid termId,
            CancellationToken cancellationToken = default);

        Task<List<GlossaryTerm>> SearchAsync(
            string searchTerm,
            bool approvedOnly = true,
            CancellationToken cancellationToken = default);
    }
}
