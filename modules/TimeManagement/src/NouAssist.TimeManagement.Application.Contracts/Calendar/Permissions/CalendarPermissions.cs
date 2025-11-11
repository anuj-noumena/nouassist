namespace NouAssist.TimeManagement.Calendar.Permissions
{
    public static class CalendarPermissions
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
}
