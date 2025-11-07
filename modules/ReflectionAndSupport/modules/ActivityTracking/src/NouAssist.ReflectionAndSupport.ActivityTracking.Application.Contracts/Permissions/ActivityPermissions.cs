namespace NouAssist.ReflectionAndSupport.ActivityTracking.Permissions
{
    public static class ActivityPermissions
    {
        public const string GroupName = "Activity";

        public static class ActivityFeeds
        {
            public const string Default = GroupName + ".ActivityFeeds";
            public const string ViewAll = Default + ".ViewAll";
            public const string Manage = Default + ".Manage";
        }

        public static class AuditLogs
        {
            public const string Default = GroupName + ".AuditLogs";
            public const string View = Default + ".View";
            public const string ViewAll = Default + ".ViewAll";
            public const string Export = Default + ".Export";
        }

        public static class EntityHistory
        {
            public const string Default = GroupName + ".EntityHistory";
            public const string View = Default + ".View";
        }
    }
}
