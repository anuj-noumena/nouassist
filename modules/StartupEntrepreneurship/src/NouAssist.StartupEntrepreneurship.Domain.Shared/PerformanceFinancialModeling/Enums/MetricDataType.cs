namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums
{
    /// <summary>
    /// Data types for metric values
    /// </summary>
    public enum MetricDataType
    {
        /// <summary>
        /// Numeric value (integer or decimal)
        /// </summary>
        Numeric = 1,
        
        /// <summary>
        /// Percentage value (0-100%)
        /// </summary>
        Percentage = 2,
        
        /// <summary>
        /// Currency amount
        /// </summary>
        Currency = 3,
        
        /// <summary>
        /// Count/quantity
        /// </summary>
        Count = 4,
        
        /// <summary>
        /// Ratio (e.g., 3:1)
        /// </summary>
        Ratio = 5,
        
        /// <summary>
        /// Duration (time-based)
        /// </summary>
        Duration = 6
    }
}
