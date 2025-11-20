namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums
{
    /// <summary>
    /// Types of financial scenarios
    /// </summary>
    public enum ScenarioType
    {
        /// <summary>
        /// Best case scenario
        /// </summary>
        Optimistic = 1,
        
        /// <summary>
        /// Most likely scenario
        /// </summary>
        Base = 2,
        
        /// <summary>
        /// Worst case scenario
        /// </summary>
        Pessimistic = 3,
        
        /// <summary>
        /// Conservative scenario
        /// </summary>
        Conservative = 4,
        
        /// <summary>
        /// Aggressive growth scenario
        /// </summary>
        Aggressive = 5,
        
        /// <summary>
        /// Custom scenario
        /// </summary>
        Custom = 6
    }
}
