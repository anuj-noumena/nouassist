namespace NouAssist.RequirementsManagement.ChangeManagement.Permissions
{
    public static class ChangeManagementPermissions
    {
        public const string GroupName = "ChangeManagement";

        // Permissions for ChangeRequest Management
        public static class ChangeRequest
        {
            public const string Default = GroupName + ".ChangeRequest"; // Group permission
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View"; // Permission to view details and lists
            public const string Approve = Default + ".Approve"; // Permission to approve changes
            public const string Reject = Default + ".Reject"; // Permission to reject changes
            public const string AssessImpact = Default + ".AssessImpact"; // Permission to assess impact
        }
    }
}
