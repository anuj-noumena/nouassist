namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos
{
    /// <summary>
    /// DTO for refresh interval
    /// </summary>
    public class RefreshIntervalDto
    {
        /// <summary>
        /// Interval value
        /// </summary>
        public int Value { get; set; }
        
        /// <summary>
        /// Unit of the interval (seconds, minutes, hours, days)
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        /// Whether automatic refresh is enabled
        /// </summary>
        public bool AutoRefreshEnabled { get; set; }
    }
}
