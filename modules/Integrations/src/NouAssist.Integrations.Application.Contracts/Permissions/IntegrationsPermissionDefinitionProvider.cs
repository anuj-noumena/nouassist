using NouAssist.Integrations.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Integrations.Permissions;

public class IntegrationsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(IntegrationsPermissions.GroupName, L("Permission:Integrations"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IntegrationsResource>(name);
    }
}
