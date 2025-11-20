namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos
{
    /// <summary>
    /// DTO for alert condition logic
    /// </summary>
    public class AlertConditionLogicDto
    {
        /// <summary>
        /// The expression that defines when an alert should trigger
        /// </summary>
        public string Expression { get; set; }
        
        /// <summary>
        /// Description of what the alert means
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Whether consecutive occurrences are required to trigger
        /// </summary>
        public bool RequireConsecutiveOccurrences { get; set; }
        
        /// <summary>
        /// Number of consecutive occurrences required
        /// </summary>
        public int ConsecutiveOccurrencesRequired { get; set; }
    }
}
