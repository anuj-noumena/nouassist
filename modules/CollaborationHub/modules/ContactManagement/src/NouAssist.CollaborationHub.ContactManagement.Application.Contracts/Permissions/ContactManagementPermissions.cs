using Volo.Abp.Reflection;

namespace NouAssist.CollaborationHub.ContactManagement.Permissions;

public class ContactManagementPermissions
{
    public const string GroupName = "ContactManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ContactManagementPermissions));
    }
}
