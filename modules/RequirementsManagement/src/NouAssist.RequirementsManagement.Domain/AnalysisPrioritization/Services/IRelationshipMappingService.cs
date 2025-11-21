using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Services
{
    /// <summary>
    /// Domain service interface for managing requirement relationships and detecting circular dependencies
    /// </summary>
    public interface IRelationshipMappingService
    {
        Task<bool> ValidateRelationshipAsync(
            Guid sourceRequirementId,
            Guid targetRequirementId,
            RelationshipType relationshipType,
            CancellationToken cancellationToken = default);

        Task<bool> WouldCreateCircularDependencyAsync(
            Guid sourceRequirementId,
            Guid targetRequirementId,
            CancellationToken cancellationToken = default);

        Task<List<Guid>> GetDependencyChainAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default);

        Task<List<Guid>> GetRelatedRequirementsAsync(
            Guid requirementId,
            RelationshipType relationshipType,
            CancellationToken cancellationToken = default);
    }
}
