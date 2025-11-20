namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Events
{
    /// <summary>
    /// Event published when marketing channel performance changes significantly
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.GrowthExperimentation.ChannelPerformanceChanged")]
    public class ChannelPerformanceChangedEto
    {
        /// <summary>
        /// The ID of the marketing channel
        /// </summary>
        public Guid ChannelId { get; set; }
        
        /// <summary>
        /// The name of the marketing channel
        /// </summary>
        public string ChannelName { get; set; }
        
        /// <summary>
        /// The type of the marketing channel
        /// </summary>
        public MarketingChannelType ChannelType { get; set; }
        
        /// <summary>
        /// The metric that changed significantly
        /// </summary>
        public string MetricName { get; set; }
        
        /// <summary>
        /// The previous value of the metric
        /// </summary>
        public double PreviousValue { get; set; }
        
        /// <summary>
        /// The new value of the metric
        /// </summary>
        public double NewValue { get; set; }
        
        /// <summary>
        /// The percentage change
        /// </summary>
        public double PercentageChange { get; set; }
        
        /// <summary>
        /// When the change was detected
        /// </summary>
        public DateTime DetectedAt { get; set; }
        
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
