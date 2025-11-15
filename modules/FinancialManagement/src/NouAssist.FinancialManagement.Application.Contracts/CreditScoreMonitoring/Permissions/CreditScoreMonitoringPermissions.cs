namespace NouAssist.CreditScoreMonitoring.Permissions
{
    public static class CreditScoreMonitoringPermissions
    {
        public const string GroupName = "CreditScoreMonitoring";

        public static class CreditReports
        {
            public const string Default = GroupName + ".CreditReports";
            public const string View = Default + ".View";
            public const string ViewDetails = Default + ".ViewDetails";
            public const string Import = Default + ".Import";
        }

        public static class CreditDisputes
        {
            public const string Default = GroupName + ".CreditDisputes";
            public const string Create = Default + ".Create";
            public const string View = Default + ".View";
            public const string Update = Default + ".Update";
            public const string Cancel = Default + ".Cancel";
        }
    }
}
