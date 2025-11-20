namespace NouAssist.Analytics.Permissions
{
    public static class AnalyticsPermissions
    {
        public const string GroupName = "Analytics";
        
        public static class Dashboards
        {
            public const string Default = GroupName + ".Dashboards";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
        
        public static class Reports
        {
            public const string Default = GroupName + ".Reports";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Generate = Default + ".Generate";
        }
        
        public static class KPIs
        {
            public const string Default = GroupName + ".KPIs";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
        
        public static class DataSources
        {
            public const string Default = GroupName + ".DataSources";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}
