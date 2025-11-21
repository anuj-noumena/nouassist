namespace NouAssist.IntelligenceStrategy.DecisionSupport.Enums
{
    /// <summary>
    /// Stage of an innovation idea in its lifecycle
    /// </summary>
    public enum IdeaStage
    {
        /// <summary>
        /// Initial capture of the idea
        /// </summary>
        Captured = 1,
        
        /// <summary>
        /// Idea is being refined and developed
        /// </summary>
        InDevelopment = 2,
        
        /// <summary>
        /// Idea is being evaluated for feasibility
        /// </summary>
        UnderEvaluation = 3,
        
        /// <summary>
        /// Idea has been prioritized for implementation
        /// </summary>
        Prioritized = 4,
        
        /// <summary>
        /// Idea has been selected for implementation
        /// </summary>
        Selected = 5,
        
        /// <summary>
        /// Idea has been rejected
        /// </summary>
        Rejected = 6
    }
}
