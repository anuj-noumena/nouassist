using Volo.Abp.Reflection;

namespace NouAssist.Collaboration.Permissions;

public class CollaborationPermissions
{
    public const string GroupName = "Collaboration";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CollaborationPermissions));
    }

    // Comments Permissions
    public static class Comments
    {
        public const string GroupName = "Comments";

        public static class Comment
        {
            public const string Default = GroupName + ".Comments";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Pin = Default + ".Pin";
            public const string React = Default + ".React";
            public const string Moderate = Default + ".Moderate";
        }
    }
}
