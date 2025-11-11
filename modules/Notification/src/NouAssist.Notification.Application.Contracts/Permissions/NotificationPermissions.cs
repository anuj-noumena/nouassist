using Volo.Abp.Reflection;

namespace NouAssist.Notification.Permissions;

public class NotificationPermissions
{
    public const string GroupName = "Notification";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(NotificationPermissions));
    }

    // Notifications Permissions
    public static class Notifications
    {
        public const string GroupName = "Notifications";

        public static class Notifications_
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

    // Reminders Permissions
    public static class Reminders
    {
        public const string GroupName = "Reminders";

        public static class Reminders_
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
