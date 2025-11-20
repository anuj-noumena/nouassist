namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Permissions
{
    public static class PerformanceFinancialModelingPermissions
    {
        public const string GroupName = "PerformanceFinancialModeling";

        // Tracked Metrics Management
        public static class TrackedMetrics
        {
            public const string Default = GroupName + ".TrackedMetrics";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageDataPoints = Default + ".ManageDataPoints";
            public const string ManageGoals = Default + ".ManageGoals";
        }

        // Metric Definitions Management
        public static class MetricDefinitions
        {
            public const string Default = GroupName + ".MetricDefinitions";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
        }

        // Metric Categories Management
        public static class MetricCategories
        {
            public const string Default = GroupName + ".MetricCategories";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
        }

        // Financial Models Management
        public static class FinancialModels
        {
            public const string Default = GroupName + ".FinancialModels";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageScenarios = Default + ".ManageScenarios";
            public const string ManageAssumptions = Default + ".ManageAssumptions";
            public const string Calculate = Default + ".Calculate";
        }

        // Financial Calculations
        public static class FinancialCalculations
        {
            public const string Default = GroupName + ".FinancialCalculations";
            public const string Execute = Default + ".Execute";
            public const string ViewResults = Default + ".ViewResults";
            public const string SensitivityAnalysis = Default + ".SensitivityAnalysis";
            public const string RunwayProjections = Default + ".RunwayProjections";
        }

        // Analytics and Reporting
        public static class Analytics
        {
            public const string Default = GroupName + ".Analytics";
            public const string ViewTrends = Default + ".ViewTrends";
            public const string ViewDashboard = Default + ".ViewDashboard";
            public const string ExportData = Default + ".ExportData";
            public const string ViewReports = Default + ".ViewReports";
        }
    }
}
