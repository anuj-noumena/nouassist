namespace NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Services
{
    /// <summary>
    /// Interface for the decision model manager domain service
    /// </summary>
    public interface IDecisionModelManager
    {
        /// <summary>
        /// Creates a new decision model instance
        /// </summary>
        Task<DecisionModelInstance> CreateModelAsync(
            string title,
            string description,
            string modelType,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Adds a criterion to a decision model
        /// </summary>
        Task<Criterion> AddCriterionAsync(
            Guid decisionModelId,
            string name,
            string description,
            decimal weight,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Adds an alternative to a decision model
        /// </summary>
        Task<Alternative> AddAlternativeAsync(
            Guid decisionModelId,
            string name,
            string description,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Sets a score for an alternative on a specific criterion
        /// </summary>
        Task SetScoreAsync(
            Guid decisionModelId,
            Guid alternativeId,
            Guid criterionId,
            decimal score,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Finalizes a decision model and selects an alternative
        /// </summary>
        Task<DecisionModelInstance> FinalizeDecisionAsync(
            Guid decisionModelId,
            Guid selectedAlternativeId,
            CancellationToken cancellationToken = default);
    }
}
