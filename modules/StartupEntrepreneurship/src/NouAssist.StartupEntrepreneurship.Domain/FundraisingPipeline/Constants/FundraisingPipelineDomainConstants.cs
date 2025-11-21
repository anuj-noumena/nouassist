namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants
{
    /// <summary>
    /// Domain-specific constants for Fundraising Pipeline
    /// </summary>
    public static class FundraisingPipelineDomainConstants
    {
        /// <summary>
        /// Maximum number of prospects per round
        /// </summary>
        public const int MaxProspectsPerRound = 500;

        /// <summary>
        /// Maximum number of pipeline stages per round
        /// </summary>
        public const int MaxPipelineStagesPerRound = 20;

        /// <summary>
        /// Maximum number of pitch deck versions
        /// </summary>
        public const int MaxPitchDeckVersions = 50;

        /// <summary>
        /// Maximum number of term sheets per round
        /// </summary>
        public const int MaxTermSheetsPerRound = 100;

        /// <summary>
        /// Minimum target amount for a fundraising round
        /// </summary>
        public const decimal MinimumTargetAmount = 10000m;

        /// <summary>
        /// Maximum target amount for a fundraising round
        /// </summary>
        public const decimal MaximumTargetAmount = 1000000000m; // 1 billion

        /// <summary>
        /// Prospect score ranges
        /// </summary>
        public static class ProspectScores
        {
            public const int MinScore = 0;
            public const int MaxScore = 100;
            public const int QualifiedThreshold = 60;
            public const int HighPriorityThreshold = 80;
        }

        /// <summary>
        /// Interaction log constants
        /// </summary>
        public static class Interactions
        {
            public const int MaxInteractionsPerProspect = 1000;
            public const int MaxNotesLength = 5000;
        }

        /// <summary>
        /// Pitch effectiveness constants
        /// </summary>
        public static class PitchEffectiveness
        {
            public const double MinEffectivenessScore = 0.0;
            public const double MaxEffectivenessScore = 1.0;
            public const double GoodEffectivenessThreshold = 0.7;
        }

        /// <summary>
        /// Forecasting constants
        /// </summary>
        public static class Forecasting
        {
            public const int MaxForecastPeriodMonths = 24;
            public const int MinForecastPeriodMonths = 1;
            public const double DefaultSuccessProbability = 0.5;
        }
    }
}
