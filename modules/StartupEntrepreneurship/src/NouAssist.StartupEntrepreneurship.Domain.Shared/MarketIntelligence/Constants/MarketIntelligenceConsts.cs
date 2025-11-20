namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants
{
    /// <summary>
    /// Constants for Market Intelligence module
    /// </summary>
    public static class MarketIntelligenceConsts
    {
        /// <summary>
        /// Module name for localization and other references
        /// </summary>
        public const string ModuleName = "MarketIntelligence";

        /// <summary>
        /// Database table prefix
        /// </summary>
        public const string DbTablePrefix = "MI_";

        /// <summary>
        /// Database schema name
        /// </summary>
        public const string DbSchema = null;

        /// <summary>
        /// Competitor-related constants
        /// </summary>
        public static class Competitor
        {
            public const int MaxNameLength = 200;
            public const int MaxDescriptionLength = 2000;
            public const int MaxWebsiteLength = 500;
            public const int MaxIndustryLength = 100;
            public const int MaxLocationLength = 200;
            public const int MaxFundingSourceLength = 200;
            public const int MaxProductNameLength = 300;
            public const int MaxProductDescriptionLength = 1000;
        }

        /// <summary>
        /// Market trend-related constants
        /// </summary>
        public static class MarketTrend
        {
            public const int MaxTitleLength = 300;
            public const int MaxDescriptionLength = 2000;
            public const int MaxSourceUrlLength = 500;
            public const int MaxKeywordsLength = 500;
            public const int MaxImpactDescriptionLength = 1000;
        }

        /// <summary>
        /// Customer segment profile-related constants
        /// </summary>
        public static class CustomerSegment
        {
            public const int MaxNameLength = 200;
            public const int MaxDescriptionLength = 2000;
            public const int MaxDemographicValueLength = 500;
            public const int MaxPsychographicValueLength = 500;
            public const int MaxNeedDescriptionLength = 1000;
            public const int MaxCharacteristicNameLength = 200;
            public const int MaxCharacteristicValueLength = 500;
        }

        /// <summary>
        /// SWOT analysis-related constants
        /// </summary>
        public static class SwotAnalysis
        {
            public const int MaxTitleLength = 300;
            public const int MaxDescriptionLength = 2000;
            public const int MaxItemTitleLength = 200;
            public const int MaxItemDescriptionLength = 1000;
            public const int MaxActionItemLength = 500;
        }

        /// <summary>
        /// General validation constants
        /// </summary>
        public static class Validation
        {
            public const int MaxNotesLength = 5000;
            public const int MaxTagsLength = 1000;
            public const int MaxUrlLength = 2000;
            public const decimal MinConfidenceScore = 0.0m;
            public const decimal MaxConfidenceScore = 1.0m;
        }
    }
}
