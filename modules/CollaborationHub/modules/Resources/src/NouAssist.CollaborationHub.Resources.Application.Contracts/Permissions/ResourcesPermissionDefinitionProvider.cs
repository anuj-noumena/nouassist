using NouAssist.CollaborationHub.Resources.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CollaborationHub.Resources.Permissions;

public class ResourcesPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ResourcesPermissions.GroupName, L("Permission:Resources"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ResourcesResource>(name);
    }
}
