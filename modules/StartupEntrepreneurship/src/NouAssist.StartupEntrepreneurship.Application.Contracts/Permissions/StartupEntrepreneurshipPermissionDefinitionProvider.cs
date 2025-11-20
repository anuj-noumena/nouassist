using NouAssist.StartupEntrepreneurship.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.StartupEntrepreneurship.Permissions;

public class StartupEntrepreneurshipPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(StartupEntrepreneurshipPermissions.GroupName, L("Permission:StartupEntrepreneurship"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<StartupEntrepreneurshipResource>(name);
    }
}
