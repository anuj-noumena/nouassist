namespace NouAssist.Meetings.Permissions
{
    public static class MeetingsPermissions
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
