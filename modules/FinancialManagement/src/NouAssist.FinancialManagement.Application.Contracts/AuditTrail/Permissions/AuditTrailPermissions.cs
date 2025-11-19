namespace NouAssist.FinancialManagement.AuditTrail.Permissions
{
    public static class AuditTrailPermissions
    {
        public const string GroupName = "AuditTrail";

        public static class AuditEntries
        {
            public const string Default = GroupName + ".AuditEntries";
            public const string View = Default + ".View";
            public const string ViewSensitive = Default + ".ViewSensitive";
            public const string GenerateReports = Default + ".GenerateReports";
            public const string Archive = Default + ".Archive";
            public const string Delete = Default + ".Delete";
        }
    }
}
