using Volo.Abp.Reflection;

namespace NouAssist.Infrastructure.Permissions;

public class InfrastructurePermissions
{
    public const string GroupName = "Infrastructure";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(InfrastructurePermissions));
    }

    // Activity Permissions
    public static class Activity
    {
        public const string GroupName = "Activity";

        public static class ActivityFeeds
        {
            public const string Default = GroupName + ".ActivityFeeds";
            public const string ViewAll = Default + ".ViewAll";
            public const string Manage = Default + ".Manage";
        }

        public static class AuditLogs
        {
            public const string Default = GroupName + ".AuditLogs";
            public const string View = Default + ".View";
            public const string ViewAll = Default + ".ViewAll";
            public const string Export = Default + ".Export";
        }

        public static class EntityHistory
        {
            public const string Default = GroupName + ".EntityHistory";
            public const string View = Default + ".View";
        }
    }

    // Search Permissions
    public static class Search
    {
        public const string GroupName = "Search";

        public static class SearchFeatures
        {
            public const string Default = GroupName + ".Search";
            public const string SaveSearches = Default + ".SaveSearches";
            public const string ViewHistory = Default + ".ViewHistory";
            public const string ViewAnalytics = Default + ".ViewAnalytics";
        }
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
