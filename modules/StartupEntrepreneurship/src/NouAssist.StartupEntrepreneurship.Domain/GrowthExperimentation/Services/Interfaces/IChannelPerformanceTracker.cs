namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services.Interfaces
{
    /// <summary>
    /// Service for tracking marketing channel performance
    /// </summary>
    public interface IChannelPerformanceTracker
    {
        /// <summary>
        /// Creates a new marketing channel
        /// </summary>
        Task<MarketingChannelPerformance> CreateChannelAsync(
            string name,
            string description,
            MarketingChannelType type,
            ChannelDetails details);
            
        /// <summary>
        /// Adds a metric to a channel
        /// </summary>
        Task<ChannelMetric> AddMetricAsync(
            Guid channelId,
            string name,
            MetricType type,
            double initialValue,
            double targetValue);
            
        /// <summary>
        /// Updates a metric value
        /// </summary>
        Task UpdateMetricValueAsync(
            Guid channelId,
            Guid metricId,
            double newValue);
            
        /// <summary>
        /// Updates cost per acquisition data
        /// </summary>
        Task UpdateCostPerAcquisitionAsync(
            Guid channelId,
            decimal cost,
            int acquisitions,
            DateTime periodStart,
            DateTime periodEnd);
            
        /// <summary>
        /// Updates return on ad spend data
        /// </summary>
        Task UpdateReturnOnAdSpendAsync(
            Guid channelId,
            decimal adSpend,
            decimal revenue,
            DateTime periodStart,
            DateTime periodEnd);
    }
}
