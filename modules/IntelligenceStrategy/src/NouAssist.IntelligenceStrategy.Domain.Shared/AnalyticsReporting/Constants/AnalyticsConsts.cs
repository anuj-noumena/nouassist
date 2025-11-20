namespace NouAssist.Analytics.Constants
{
    /// <summary>
    /// Contains constants for the Analytics module
    /// </summary>
    public static class AnalyticsConsts
    {
        public const string ModuleName = "Analytics";
        
        public static class Dashboard
        {
            public const int MaxNameLength = 128;
            public const int MaxDescriptionLength = 512;
            public const int MaxWidgetsPerDashboard = 20;
        }
        
        public static class Report
        {
            public const int MaxNameLength = 128;
            public const int MaxDescriptionLength = 1024;
            public const int MaxParametersPerReport = 15;
        }
        
        public static class Kpi
        {
            public const int MaxNameLength = 128;
            public const int MaxDescriptionLength = 512;
            public const int MaxThresholdsPerKpi = 5;
        }
        
        public static class DataSource
        {
            public const int MaxNameLength = 128;
            public const int MaxConnectionStringLength = 512;
            public const int MaxTransformationStepsPerDataSource = 10;
        }
    }
}
