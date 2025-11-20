namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Permissions
{
    /// <summary>
    /// Permission definitions for the Analytics & Reporting module
    /// </summary>
    public static class AnalyticsReportingPermissions
    {
        /// <summary>
        /// The name of the module for grouping permissions
        /// </summary>
        public const string GroupName = "AnalyticsReporting";

        /// <summary>
        /// Dashboard permissions
        /// </summary>
        public static class Dashboard
        {
            /// <summary>
            /// Root permission for dashboards
            /// </summary>
            public const string Default = GroupName + ".Dashboard";
            
            /// <summary>
            /// Permission to create dashboards
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to update dashboards
            /// </summary>
            public const string Update = Default + ".Update";
            
            /// <summary>
            /// Permission to delete dashboards
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to view dashboards
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to add widgets to dashboards
            /// </summary>
            public const string AddWidget = Default + ".AddWidget";
        }

        /// <summary>
        /// Report permissions
        /// </summary>
        public static class Report
        {
            /// <summary>
            /// Root permission for reports
            /// </summary>
            public const string Default = GroupName + ".Report";
            
            /// <summary>
            /// Permission to create reports
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to update reports
            /// </summary>
            public const string Update = Default + ".Update";
            
            /// <summary>
            /// Permission to delete reports
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to view reports
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to generate reports
            /// </summary>
            public const string Generate = Default + ".Generate";
            
            /// <summary>
            /// Permission to schedule reports
            /// </summary>
            public const string Schedule = Default + ".Schedule";
        }

        /// <summary>
        /// KPI permissions
        /// </summary>
        public static class Kpi
        {
            /// <summary>
            /// Root permission for KPIs
            /// </summary>
            public const string Default = GroupName + ".Kpi";
            
            /// <summary>
            /// Permission to create KPIs
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to update KPIs
            /// </summary>
            public const string Update = Default + ".Update";
            
            /// <summary>
            /// Permission to delete KPIs
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to view KPIs
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to manage thresholds for KPIs
            /// </summary>
            public const string ManageThresholds = Default + ".ManageThresholds";
        }

        /// <summary>
        /// DataSource permissions
        /// </summary>
        public static class DataSource
        {
            /// <summary>
            /// Root permission for data sources
            /// </summary>
            public const string Default = GroupName + ".DataSource";
            
            /// <summary>
            /// Permission to create data sources
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to update data sources
            /// </summary>
            public const string Update = Default + ".Update";
            
            /// <summary>
            /// Permission to delete data sources
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to view data sources
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to configure data transformations
            /// </summary>
            public const string ConfigureTransformations = Default + ".ConfigureTransformations";
        }
    }
}
