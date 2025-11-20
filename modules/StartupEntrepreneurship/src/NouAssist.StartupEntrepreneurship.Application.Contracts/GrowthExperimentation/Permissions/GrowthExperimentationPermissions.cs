namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Permissions
{
    public static class GrowthExperimentationPermissions
    {
        public const string GroupName = "GrowthExperimentation";

        // Permissions for GrowthExperiment Management
        public static class GrowthExperiment
        {
            public const string Default = GroupName + ".GrowthExperiment"; // Group permission
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View"; 
            public const string ManageResults = Default + ".ManageResults";
            public const string AnalyzeResults = Default + ".AnalyzeResults";
        }

        // Permissions for MarketingChannel Management
        public static class MarketingChannel
        {
            public const string Default = GroupName + ".MarketingChannel"; 
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View"; 
            public const string ManageMetrics = Default + ".ManageMetrics";
        }

        // Permissions for ConversionFunnel Management
        public static class ConversionFunnel
        {
            public const string Default = GroupName + ".ConversionFunnel"; 
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View"; 
            public const string ManageStages = Default + ".ManageStages";
        }
    }
}
