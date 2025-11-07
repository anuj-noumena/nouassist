using Volo.Abp.Reflection;

namespace NouAssist.CollaborationHub.UserProfiles.Permissions;

public class UserProfilesPermissions
{
    public const string GroupName = "UserProfiles";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(UserProfilesPermissions));
    }
}
