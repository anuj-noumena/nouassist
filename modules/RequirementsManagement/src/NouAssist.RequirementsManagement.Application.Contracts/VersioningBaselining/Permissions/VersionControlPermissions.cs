namespace NouAssist.RequirementsManagement.VersioningBaselining.Permissions
{
    public static class VersionControlPermissions
    {
        public const string GroupName = "VersionControl";

        // Permissions for Versioning
        public static class Versioning
        {
            public const string Default = GroupName + ".Versioning";
            public const string Create = Default + ".Create";
            public const string View = Default + ".View";
            public const string Compare = Default + ".Compare";
        }

        // Permissions for Baselining
        public static class Baselining
        {
            public const string Default = GroupName + ".Baselining";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageStatus = Default + ".ManageStatus";
        }

        // Permissions for Branching
        public static class Branching
        {
            public const string Default = GroupName + ".Branching";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Merge = Default + ".Merge";
        }
    }
}
