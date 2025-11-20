namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums
{
    /// <summary>
    /// Types of metrics that can be tracked
    /// </summary>
    public enum MetricType
    {
        /// <summary>
        /// Financial metrics (revenue, profit, burn rate, etc.)
        /// </summary>
        Financial = 1,
        
        /// <summary>
        /// Customer metrics (acquisition, retention, churn, etc.)
        /// </summary>
        Customer = 2,
        
        /// <summary>
        /// Product metrics (usage, engagement, features, etc.)
        /// </summary>
        Product = 3,
        
        /// <summary>
        /// Operational metrics (efficiency, costs, resources, etc.)
        /// </summary>
        Operational = 4,
        
        /// <summary>
        /// Marketing metrics (CAC, LTV, conversion rates, etc.)
        /// </summary>
        Marketing = 5,
        
        /// <summary>
        /// HR metrics (headcount, productivity, satisfaction, etc.)
        /// </summary>
        HumanResources = 6
    }
}
