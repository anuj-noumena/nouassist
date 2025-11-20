namespace NouAssist.IntelligenceStrategy.DecisionSupport.Enums
{
    /// <summary>
    /// Represents the status of a strategic initiative proposal
    /// </summary>
    public enum ProposalStatus
    {
        /// <summary>
        /// Proposal has been submitted but not yet reviewed
        /// </summary>
        Proposed = 0,
        
        /// <summary>
        /// Proposal has been approved and can proceed to project creation
        /// </summary>
        Approved = 1,
        
        /// <summary>
        /// Proposal has been rejected and will not proceed further
        /// </summary>
        Rejected = 2
    }
}
