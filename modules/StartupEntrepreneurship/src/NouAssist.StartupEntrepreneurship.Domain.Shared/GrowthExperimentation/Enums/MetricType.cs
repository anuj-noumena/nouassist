namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums
{
    /// <summary>
    /// Represents different types of metrics that can be measured in experiments
    /// </summary>
    public enum MetricType
    {
        /// <summary>
        /// Conversion rate (percentage)
        /// </summary>
        ConversionRate = 0,
        
        /// <summary>
        /// Click-through rate (percentage)
        /// </summary>
        ClickThroughRate = 1,
        
        /// <summary>
        /// Average revenue per user
        /// </summary>
        AverageRevenuePerUser = 2,
        
        /// <summary>
        /// Cost per acquisition
        /// </summary>
        CostPerAcquisition = 3,
        
        /// <summary>
        /// Return on ad spend
        /// </summary>
        ReturnOnAdSpend = 4,
        
        /// <summary>
        /// User retention rate
        /// </summary>
        RetentionRate = 5,
        
        /// <summary>
        /// Bounce rate (percentage)
        /// </summary>
        BounceRate = 6,
        
        /// <summary>
        /// Custom metric defined by the user
        /// </summary>
        Custom = 7
    }
}
