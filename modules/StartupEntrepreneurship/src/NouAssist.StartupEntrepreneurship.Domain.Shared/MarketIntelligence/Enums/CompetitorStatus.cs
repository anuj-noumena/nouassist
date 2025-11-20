namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums
{
    /// <summary>
    /// Defines the status of a competitor in our tracking system
    /// </summary>
    public enum CompetitorStatus
    {
        /// <summary>
        /// Competitor is actively monitored
        /// </summary>
        Active = 1,

        /// <summary>
        /// Competitor monitoring is paused
        /// </summary>
        Inactive = 2,

        /// <summary>
        /// Competitor is no longer relevant
        /// </summary>
        Archived = 3,

        /// <summary>
        /// Competitor is under evaluation
        /// </summary>
        PendingReview = 4
    }
}
