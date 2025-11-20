namespace NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Services
{
    /// <summary>
    /// Interface for the strategic initiative manager domain service
    /// </summary>
    public interface IStrategicInitiativeManager
    {
        /// <summary>
        /// Creates a new strategic initiative proposal
        /// </summary>
        Task<StrategicInitiativeProposal> CreateProposalAsync(
            string title,
            string description,
            DecisionRationaleText rationale,
            ExpectedOutcomeText expectedOutcomes,
            Guid? relatedDecisionModelId = null,
            AlignmentStatus strategicAlignment = AlignmentStatus.NotEvaluated,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Approves a strategic initiative proposal
        /// </summary>
        Task<StrategicInitiativeProposal> ApproveProposalAsync(
            Guid proposalId,
            Guid? projectId = null,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Rejects a strategic initiative proposal
        /// </summary>
        Task<StrategicInitiativeProposal> RejectProposalAsync(
            Guid proposalId,
            string rejectionReason,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Creates a strategic initiative from a decision model
        /// </summary>
        Task<StrategicInitiativeProposal> CreateFromDecisionModelAsync(
            Guid decisionModelId,
            string title,
            string description,
            DecisionRationaleText rationale,
            ExpectedOutcomeText expectedOutcomes,
            AlignmentStatus alignment = AlignmentStatus.NotEvaluated,
            CancellationToken cancellationToken = default);
    }
}
