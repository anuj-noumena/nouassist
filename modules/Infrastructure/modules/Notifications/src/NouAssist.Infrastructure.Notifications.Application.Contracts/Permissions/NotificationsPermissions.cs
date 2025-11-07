namespace NouAssist.Infrastructure.Notifications.Permissions
{
    public static class NotificationsPermissions
    {
        public const string GroupName = "Notifications";

        public static class Notifications
        {
            public const string Default = GroupName + ".Notifications";
            public const string Manage = Default + ".Manage";
        }

        public static class Preferences
        {
            public const string Default = GroupName + ".Preferences";
            public const string Manage = Default + ".Manage";
        }
    }
}
