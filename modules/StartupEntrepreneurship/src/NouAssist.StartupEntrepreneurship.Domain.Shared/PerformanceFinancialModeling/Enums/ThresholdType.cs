namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums
{
    /// <summary>
    /// Types of threshold alerts for metrics
    /// </summary>
    public enum ThresholdType
    {
        /// <summary>
        /// Alert when value is above threshold
        /// </summary>
        Above = 1,
        
        /// <summary>
        /// Alert when value is below threshold
        /// </summary>
        Below = 2,
        
        /// <summary>
        /// Alert when value equals threshold
        /// </summary>
        Equals = 3,
        
        /// <summary>
        /// Alert when value is between two thresholds
        /// </summary>
        Between = 4,
        
        /// <summary>
        /// Alert when value is outside two thresholds
        /// </summary>
        Outside = 5
    }
}
