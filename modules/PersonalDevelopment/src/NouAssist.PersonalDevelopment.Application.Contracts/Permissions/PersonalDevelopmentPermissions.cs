using Volo.Abp.Reflection;

namespace NouAssist.PersonalDevelopment.Permissions;

public class PersonalDevelopmentPermissions
{
    public const string GroupName = "PersonalDevelopment";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(PersonalDevelopmentPermissions));
    }

    // Goals Permissions
    public static class Goal
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

    // Habits Permissions
    public static class Habit
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
