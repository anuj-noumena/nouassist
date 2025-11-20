namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants
{
    /// <summary>
    /// Constant values used throughout the AnalyticsReporting module
    /// </summary>
    public static class AnalyticsReportingConsts
    {
        /// <summary>
        /// Maximum name length for domain objects
        /// </summary>
        public const int MaxNameLength = 128;

        /// <summary>
        /// Maximum length for description fields
        /// </summary>
        public const int MaxDescriptionLength = 1000;

        /// <summary>
        /// Maximum length for a connection string
        /// </summary>
        public const int MaxConnectionStringLength = 512;

        /// <summary>
        /// Maximum length for encrypted credentials
        /// </summary>
        public const int MaxEncryptedCredentialsLength = 2000;

        /// <summary>
        /// Maximum length for transformation rules
        /// </summary>
        public const int MaxTransformationRuleLength = 4000;

        /// <summary>
        /// Maximum length for alert condition logic
        /// </summary>
        public const int MaxAlertConditionLogicLength = 1000;
        
        /// <summary>
        /// Maximum length for insight text
        /// </summary>
        public const int MaxInsightTextLength = 2000;
        
        /// <summary>
        /// Maximum length for recommended actions
        /// </summary>
        public const int MaxRecommendedActionLength = 1000;
    }
}
