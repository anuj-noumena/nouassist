namespace NouAssist.LearningDevelopment.SkillDevelopment.Permissions
{
    public static class SkillDevelopmentPermissions
    {
        public const string GroupName = "SkillDevelopment";

        public static class Skills
        {
            public const string Default = GroupName + ".Skills";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string ManageSubSkills = Default + ".ManageSubSkills";
        }

        public static class DevelopmentSessions
        {
            public const string Default = GroupName + ".DevelopmentSessions";
            public const string Start = Default + ".Start";
            public const string Manage = Default + ".Manage";
            public const string Complete = Default + ".Complete";
            public const string AddPracticeLog = Default + ".AddPracticeLog";
            public const string AddApplication = Default + ".AddApplication";
        }

        public static class SkillLogs
        {
            public const string Default = GroupName + ".SkillLogs";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class SkillTrees
        {
            public const string Default = GroupName + ".SkillTrees";
            public const string View = Default + ".View";
            public const string Edit = Default + ".Edit";
        }

        public static class Resources
        {
            public const string Default = GroupName + ".Resources";
            public const string Track = Default + ".Track";
            public const string View = Default + ".View";
        }
    }
}
