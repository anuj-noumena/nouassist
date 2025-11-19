using Volo.Abp.Reflection;

namespace NouAssist.AssetManagement.Permissions;

public class AssetManagementPermissions
{
    public const string GroupName = "AssetManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(AssetManagementPermissions));
    }

    // Resources Management Permissions
    public static class Resources
    {
        public const string GroupName = "Resources";

        public static class Resource
        {
            public const string Default = GroupName + ".Resources";
            public const string Upload = Default + ".Upload";
            public const string Download = Default + ".Download";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageVersions = Default + ".ManageVersions";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class Books
        {
            public const string Default = GroupName + ".Books";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string UpdateReadingProgress = Default + ".UpdateReadingProgress";
            public const string Rate = Default + ".Rate";
        }

        public static class Collections
        {
            public const string Default = GroupName + ".Collections";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class Inventory
        {
            public const string Default = GroupName + ".Inventory";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string CheckOut = Default + ".CheckOut";
            public const string CheckIn = Default + ".CheckIn";
            public const string ScheduleMaintenance = Default + ".ScheduleMaintenance";
            public const string ViewAll = Default + ".ViewAll";
        }

        public static class Storage
        {
            public const string Default = GroupName + ".Storage";
            public const string Manage = Default + ".Manage";
        }
    }
}
