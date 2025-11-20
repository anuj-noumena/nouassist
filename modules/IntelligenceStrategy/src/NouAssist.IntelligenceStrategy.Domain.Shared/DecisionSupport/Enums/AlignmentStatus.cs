namespace NouAssist.IntelligenceStrategy.DecisionSupport.Enums
{
    /// <summary>
    /// Represents the degree of alignment between a proposal and strategic goals
    /// </summary>
    public enum AlignmentStatus
    {
        /// <summary>
        /// Not yet evaluated
        /// </summary>
        NotEvaluated = 0,
        
        /// <summary>
        /// Low alignment with strategic goals
        /// </summary>
        Low = 1,
        
        /// <summary>
        /// Medium alignment with strategic goals
        /// </summary>
        Medium = 2,
        
        /// <summary>
        /// High alignment with strategic goals
        /// </summary>
        High = 3
    }
}
