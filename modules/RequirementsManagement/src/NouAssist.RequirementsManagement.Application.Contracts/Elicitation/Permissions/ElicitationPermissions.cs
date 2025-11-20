namespace NouAssist.RequirementsManagement.Elicitation.Permissions
{
    public static class ElicitationPermissions
    {
        public const string GroupName = "Elicitation";

        // StakeholderProfile permissions
        public static class StakeholderProfiles
        {
            public const string Default = GroupName + ".StakeholderProfiles";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageAll = Default + ".ManageAll";
        }

        // ElicitationSession permissions
        public static class ElicitationSessions
        {
            public const string Default = GroupName + ".ElicitationSessions";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Manage = Default + ".Manage";
            public const string Schedule = Default + ".Schedule";
            public const string Cancel = Default + ".Cancel";
        }

        // ElicitationArtifact permissions
        public static class ElicitationArtifacts
        {
            public const string Default = GroupName + ".ElicitationArtifacts";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Approve = Default + ".Approve";
            public const string Reject = Default + ".Reject";
        }

        // Survey permissions
        public static class Surveys
        {
            public const string Default = GroupName + ".Surveys";
            public const string Create = Default + ".Create";
            public const string Deploy = Default + ".Deploy";
            public const string ViewResults = Default + ".ViewResults";
            public const string Respond = Default + ".Respond";
        }

        // Interview permissions
        public static class Interviews
        {
            public const string Default = GroupName + ".Interviews";
            public const string Schedule = Default + ".Schedule";
            public const string Conduct = Default + ".Conduct";
            public const string ViewRecords = Default + ".ViewRecords";
        }

        // BrainstormingIdea permissions
        public static class BrainstormingIdeas
        {
            public const string Default = GroupName + ".BrainstormingIdeas";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Select = Default + ".Select";
        }
    }
}
