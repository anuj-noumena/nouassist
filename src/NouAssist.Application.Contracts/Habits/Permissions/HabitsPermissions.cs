namespace NouAssist.Habits.Permissions
{
    public static class HabitsPermissions
    {
        public const string GroupName = "Habits";

        public static class Habits
        {
            public const string Default = GroupName + ".Habits";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Track = Default + ".Track";
        }

        public static class Routines
        {
            public const string Default = GroupName + ".Routines";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Complete = Default + ".Complete";
        }

        public static class Tracking
        {
            public const string Default = GroupName + ".Tracking";
            public const string ViewOwn = Default + ".ViewOwn";
            public const string ManageOwn = Default + ".ManageOwn";
        }

        public static class Analytics
        {
            public const string Default = GroupName + ".Analytics";
            public const string ViewOwn = Default + ".ViewOwn";
        }
    }
}
