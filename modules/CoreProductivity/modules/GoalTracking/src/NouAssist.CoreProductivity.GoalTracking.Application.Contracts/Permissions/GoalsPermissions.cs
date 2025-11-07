namespace NouAssist.CoreProductivity.GoalTracking.Permissions
{
    public static class GoalsPermissions
    {
        public const string GroupName = "Goals";

        public static class Goals
        {
            public const string Default = GroupName + ".Goals";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string UpdateProgress = Default + ".UpdateProgress";
            public const string Achieve = Default + ".Achieve";
            public const string Archive = Default + ".Archive";
        }

        public static class Achievements
        {
            public const string Default = GroupName + ".Achievements";
            public const string View = Default + ".View";
            public const string Manage = Default + ".Manage";
        }
    }
}
