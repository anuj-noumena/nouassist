using NouAssist.CollaborationHub.UserProfiles.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CollaborationHub.UserProfiles.Permissions;

public class UserProfilesPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(UserProfilesPermissions.GroupName, L("Permission:UserProfiles"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<UserProfilesResource>(name);
    }
}
