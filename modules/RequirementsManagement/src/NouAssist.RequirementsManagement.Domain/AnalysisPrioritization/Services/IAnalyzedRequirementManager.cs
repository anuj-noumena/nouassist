namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Services
{
    /// <summary>
    /// Domain service interface for managing analyzed requirements
    /// </summary>
    public interface IAnalyzedRequirementManager
    {
        Task<AnalyzedRequirement> CreateOrUpdateAsync(
            Guid requirementId,
            string requirementTitle,
            CancellationToken cancellationToken = default);

        Task SetAttributeAsync(
            Guid analyzedRequirementId,
            string attributeName,
            AttributeType attributeType,
            AttributeValue attributeValue,
            CancellationToken cancellationToken = default);

        Task RemoveAttributeAsync(
            Guid analyzedRequirementId,
            string attributeName,
            CancellationToken cancellationToken = default);

        Task AddRelationshipAsync(
            Guid sourceRequirementId,
            Guid targetRequirementId,
            RelationshipType relationshipType,
            string? description = null,
            CancellationToken cancellationToken = default);

        Task RemoveRelationshipAsync(
            Guid analyzedRequirementId,
            Guid relationshipId,
            CancellationToken cancellationToken = default);

        Task SetImpactAssessmentAsync(
            Guid analyzedRequirementId,
            ImpactAssessmentResult impactAssessment,
            CancellationToken cancellationToken = default);

        Task<List<AnalyzedRequirement>> GetRequirementsNeedingAnalysisAsync(
            CancellationToken cancellationToken = default);
    }
}
