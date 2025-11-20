namespace NouAssist.BusinessOperations.FreelanceOperations.Permissions
{
    public static class FreelanceOperationsPermissions
    {
        public const string GroupName = "FreelanceOperations";

        // Project permissions
        public static class Project
        {
            public const string Default = GroupName + ".Project";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageTimeEntries = Default + ".ManageTimeEntries";
            public const string ManageMilestones = Default + ".ManageMilestones";
        }

        // Client permissions
        public static class Client
        {
            public const string Default = GroupName + ".Client";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageCommunications = Default + ".ManageCommunications";
        }

        // Proposal permissions
        public static class Proposal
        {
            public const string Default = GroupName + ".Proposal";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Send = Default + ".Send";
            public const string ManageVersions = Default + ".ManageVersions";
        }

        // Upwork permissions
        public static class Upwork
        {
            public const string Default = GroupName + ".Upwork";
            public const string ManageCredentials = Default + ".ManageCredentials";
            public const string ManageEngagements = Default + ".ManageEngagements";
            public const string ImportJobs = Default + ".ImportJobs";
            public const string View = Default + ".View";
        }
    }
}
