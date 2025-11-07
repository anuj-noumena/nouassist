using NouAssist.CollaborationHub.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CollaborationHub.Permissions;

public class CollaborationHubPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CollaborationHubPermissions.GroupName, L("Permission:CollaborationHub"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CollaborationHubResource>(name);
    }
}
