namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants
{
    /// <summary>
    /// Constants for the GrowthExperimentation module
    /// </summary>
    public static class GrowthExperimentationConstants
    {
        /// <summary>
        /// Name of the module
        /// </summary>
        public const string ModuleName = "GrowthExperimentation";
        
        public static class Experiments
        {
            public const int MaxNameLength = 128;
            public const int MaxDescriptionLength = 1000;
            public const int MaxHypothesisTextLength = 500;
            public const int MaxLearningsTextLength = 2000;
            public const int MaxVariantNameLength = 100;
            public const int MinVariantsCount = 2;
            public const double MinSignificanceThreshold = 0.8;
            public const double DefaultSignificanceLevel = 0.95;
        }
        
        public static class MarketingChannels
        {
            public const int MaxChannelNameLength = 100;
            public const int MaxChannelDescriptionLength = 500;
        }
        
        public static class ConversionFunnels
        {
            public const int MaxFunnelNameLength = 100;
            public const int MaxFunnelDescriptionLength = 500;
            public const int MaxStageNameLength = 100;
            public const int MinFunnelStages = 2;
            public const int MaxFunnelStages = 10;
        }
    }
}
