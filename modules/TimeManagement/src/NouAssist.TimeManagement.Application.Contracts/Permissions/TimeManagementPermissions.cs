using Volo.Abp.Reflection;

namespace NouAssist.TimeManagement.Permissions;

public class TimeManagementPermissions
{
    public const string GroupName = "TimeManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(TimeManagementPermissions));
    }

    // Calendar Permissions
    public static class Calendar
    {
        public const string GroupName = "Calendar";

        public static class Calendars
        {
            public const string Default = GroupName + ".Calendars";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Share = Default + ".Share";
        }

        public static class CalendarEvents
        {
            public const string Default = GroupName + ".CalendarEvents";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Cancel = Default + ".Cancel";
        }

        public static class Timeblocks
        {
            public const string Default = GroupName + ".Timeblocks";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Complete = Default + ".Complete";
        }

        public static class SchedulingRequests
        {
            public const string Default = GroupName + ".SchedulingRequests";
            public const string Create = Default + ".Create";
            public const string Schedule = Default + ".Schedule";
        }
    }

    // Meetings Permissions
    public static class Meetings
    {
        public const string GroupName = "Meetings";

        public static class Meetings
        {
            public const string Default = GroupName + ".Meetings";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Cancel = Default + ".Cancel";
            public const string Start = Default + ".Start";
            public const string Complete = Default + ".Complete";
            public const string ManageAttendees = Default + ".ManageAttendees";
            public const string ManageAgenda = Default + ".ManageAgenda";
        }
    }
}
