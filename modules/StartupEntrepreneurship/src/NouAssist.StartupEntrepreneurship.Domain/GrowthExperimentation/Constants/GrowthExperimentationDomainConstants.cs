namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants
{
    /// <summary>
    /// Domain-specific constants for Growth Experimentation
    /// </summary>
    public static class GrowthExperimentationDomainConstants
    {
        /// <summary>
        /// Maximum number of experiments that can run concurrently
        /// </summary>
        public const int MaxConcurrentExperiments = 10;

        /// <summary>
        /// Minimum number of variants per experiment
        /// </summary>
        public const int MinVariantsPerExperiment = 2;

        /// <summary>
        /// Maximum number of variants per experiment
        /// </summary>
        public const int MaxVariantsPerExperiment = 10;

        /// <summary>
        /// Maximum number of target metrics per experiment
        /// </summary>
        public const int MaxTargetMetricsPerExperiment = 20;

        /// <summary>
        /// Maximum number of hypotheses per experiment
        /// </summary>
        public const int MaxHypothesesPerExperiment = 10;

        /// <summary>
        /// Minimum experiment duration in days
        /// </summary>
        public const int MinExperimentDurationDays = 1;

        /// <summary>
        /// Maximum experiment duration in days
        /// </summary>
        public const int MaxExperimentDurationDays = 365;

        /// <summary>
        /// Statistical significance thresholds
        /// </summary>
        public static class StatisticalSignificance
        {
            public const double MinPValue = 0.0;
            public const double MaxPValue = 1.0;
            public const double SignificanceThreshold = 0.05; // 95% confidence
            public const double HighConfidenceThreshold = 0.01; // 99% confidence
        }

        /// <summary>
        /// Conversion funnel constants
        /// </summary>
        public static class ConversionFunnel
        {
            public const int MinStages = 2;
            public const int MaxStages = 20;
            public const int MaxFunnelsPerExperiment = 10;
        }

        /// <summary>
        /// Marketing channel constants
        /// </summary>
        public static class MarketingChannel
        {
            public const int MaxChannels = 100;
            public const int MaxCampaignsPerChannel = 500;
        }

        /// <summary>
        /// Learning repository constants
        /// </summary>
        public static class Learning
        {
            public const int MaxLearningsPerExperiment = 50;
            public const int MaxRelatedExperiments = 20;
        }

        /// <summary>
        /// Sample size constants
        /// </summary>
        public static class SampleSize
        {
            public const int MinSampleSizePerVariant = 100;
            public const int RecommendedMinSampleSize = 1000;
        }
    }
}
