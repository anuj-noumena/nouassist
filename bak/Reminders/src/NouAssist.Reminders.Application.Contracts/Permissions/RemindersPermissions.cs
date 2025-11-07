namespace NouAssist.Reminders.Permissions
{
    public static class RemindersPermissions
    {
        public const string GroupName = "Reminders";

        public static class Reminders
        {
            public const string Default = GroupName + ".Reminders";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Snooze = Default + ".Snooze";
            public const string ViewAll = Default + ".ViewAll";
        }
    }
}
