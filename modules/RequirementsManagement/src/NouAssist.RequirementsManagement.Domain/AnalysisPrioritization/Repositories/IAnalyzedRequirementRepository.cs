using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Repositories
{
    /// <summary>
    /// Repository interface for AnalyzedRequirement aggregate
    /// </summary>
    public interface IAnalyzedRequirementRepository : IRepository<AnalyzedRequirement, Guid>
    {
        /// <summary>
        /// Finds an analyzed requirement by the original requirement ID
        /// </summary>
        Task<AnalyzedRequirement?> FindByRequirementIdAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets analyzed requirements matching the given specification
        /// </summary>
        Task<List<AnalyzedRequirement>> GetListAsync(
            Specification<AnalyzedRequirement> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets analyzed requirements with specific attributes
        /// </summary>
        Task<List<AnalyzedRequirement>> GetRequirementsWithAttributeAsync(
            AttributeType attributeType,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets requirements that have relationships of a specific type
        /// </summary>
        Task<List<AnalyzedRequirement>> GetRequirementsWithRelationshipTypeAsync(
            RelationshipType relationshipType,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets requirements without impact assessment
        /// </summary>
        Task<List<AnalyzedRequirement>> GetRequirementsWithoutImpactAssessmentAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts requirements matching the given specification
        /// </summary>
        Task<int> CountAsync(
            Specification<AnalyzedRequirement> specification,
            CancellationToken cancellationToken = default);
    }
}
