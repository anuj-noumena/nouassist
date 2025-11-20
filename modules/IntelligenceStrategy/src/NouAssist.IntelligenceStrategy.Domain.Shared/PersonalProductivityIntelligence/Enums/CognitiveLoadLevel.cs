namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums
{
    /// <summary>
    /// Represents the cognitive load level of a user
    /// </summary>
    public enum CognitiveLoadLevel
    {
        /// <summary>
        /// Low cognitive load, indicating user has capacity for more tasks
        /// </summary>
        Low = 0,
        
        /// <summary>
        /// Medium cognitive load, indicating user is at optimal productivity
        /// </summary>
        Medium = 1,
        
        /// <summary>
        /// High cognitive load, indicating user may be approaching burnout
        /// </summary>
        High = 2,
        
        /// <summary>
        /// Critical cognitive load, indicating user is overwhelmed
        /// </summary>
        Critical = 3
    }
}
