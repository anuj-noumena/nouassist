using Volo.Abp.Reflection;

namespace NouAssist.Customization.Permissions;

public class CustomizationPermissions
{
    public const string GroupName = "Customization";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CustomizationPermissions));
    }

    // CustomFields Permissions
    public static class CustomFields
    {
        public const string GroupName = "CustomFields";

        public static class Definitions
        {
            public const string Default = GroupName + ".Definitions";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Activate = Default + ".Activate";
            public const string Deactivate = Default + ".Deactivate";
        }

        public static class Values
        {
            public const string Default = GroupName + ".Values";
            public const string Manage = Default + ".Manage";
        }
    }
}
