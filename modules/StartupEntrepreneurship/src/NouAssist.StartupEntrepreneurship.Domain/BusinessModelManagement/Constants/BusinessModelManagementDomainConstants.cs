namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants
{
    /// <summary>
    /// Domain-specific constants for Business Model Management
    /// </summary>
    public static class BusinessModelManagementDomainConstants
    {
        /// <summary>
        /// Maximum number of versions per canvas
        /// </summary>
        public const int MaxVersionsPerCanvas = 100;

        /// <summary>
        /// Maximum number of components per canvas
        /// </summary>
        public const int MaxComponentsPerCanvas = 50;

        /// <summary>
        /// Maximum number of hypotheses that can be tracked
        /// </summary>
        public const int MaxHypothesesPerCanvas = 50;

        /// <summary>
        /// Minimum days between strategic pivots
        /// </summary>
        public const int MinDaysBetweenPivots = 30;

        /// <summary>
        /// Default importance for canvas validation
        /// </summary>
        public const double DefaultImportance = 0.5;

        /// <summary>
        /// Canvas validation score thresholds
        /// </summary>
        public static class ValidationScores
        {
            public const int MinimumScoreForValidated = 70;
            public const int MinimumScoreForTested = 50;
            public const int MaxScore = 100;
        }

        /// <summary>
        /// Revenue model constants
        /// </summary>
        public static class RevenueModel
        {
            public const int MaxPricingTiersPerModel = 10;
            public const int MaxScenariosPerModel = 20;
            public const decimal MinimumRevenueAmount = 0.01m;
        }

        /// <summary>
        /// Strategic pivot constants
        /// </summary>
        public static class Pivot
        {
            public const int MaxActivePivotsPerCanvas = 3;
            public const int MaxHistoricalPivots = 50;
        }
    }
}
