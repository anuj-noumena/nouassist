using Volo.Abp.Reflection;

namespace NouAssist.BusinessOperations.DocumentManagement.Permissions
{
    /// <summary>
    /// Defines permission constants for document management
    /// </summary>
    public class DocumentManagementPermissions
    {
        public const string GroupName = "DocumentManagement";

        public static class Documents
        {
            public const string Default = GroupName + ".Documents";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageAccess = Default + ".ManageAccess";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(DocumentManagementPermissions));
        }
    }
}
