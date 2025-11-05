namespace NouAssist.Approval.Permissions
{
    public static class ApprovalPermissions
    {
        public const string GroupName = "Approval";

        public static class Workflows
        {
            public const string Default = GroupName + ".Workflows";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Activate = Default + ".Activate";
            public const string Deactivate = Default + ".Deactivate";
        }

        public static class Requests
        {
            public const string Default = GroupName + ".Requests";
            public const string Create = Default + ".Create";
            public const string View = Default + ".View";
            public const string Approve = Default + ".Approve";
            public const string Reject = Default + ".Reject";
            public const string Cancel = Default + ".Cancel";
        }
    }
}
