namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Permissions
{
    public static class MarketIntelligencePermissions
    {
        public const string GroupName = "MarketIntelligence";

        // Permissions for Competitor Management
        public static class Competitor
        {
            public const string Default = GroupName + ".Competitor";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageStatus = Default + ".ManageStatus";
            public const string ManageThreatLevel = Default + ".ManageThreatLevel";
        }

        // Permissions for Market Trend Management
        public static class MarketTrend
        {
            public const string Default = GroupName + ".MarketTrend";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageDataSources = Default + ".ManageDataSources";
        }

        // Permissions for Customer Segment Profile Management
        public static class CustomerSegment
        {
            public const string Default = GroupName + ".CustomerSegment";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageCharacteristics = Default + ".ManageCharacteristics";
        }

        // Permissions for SWOT Analysis Management
        public static class SwotAnalysis
        {
            public const string Default = GroupName + ".SwotAnalysis";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageItems = Default + ".ManageItems";
            public const string Complete = Default + ".Complete";
        }

        // Permissions for Market Intelligence Dashboard
        public static class Dashboard
        {
            public const string Default = GroupName + ".Dashboard";
            public const string View = Default + ".View";
        }
    }
}
