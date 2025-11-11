using NouAssist.Collaboration.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Collaboration.Permissions;

public class CollaborationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CollaborationPermissions.GroupName, L("Permission:Collaboration"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CollaborationResource>(name);
    }
}
