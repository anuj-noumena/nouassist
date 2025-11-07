using NouAssist.Infrastructure.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Infrastructure.Permissions;

public class InfrastructurePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(InfrastructurePermissions.GroupName, L("Permission:Infrastructure"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<InfrastructureResource>(name);
    }
}
