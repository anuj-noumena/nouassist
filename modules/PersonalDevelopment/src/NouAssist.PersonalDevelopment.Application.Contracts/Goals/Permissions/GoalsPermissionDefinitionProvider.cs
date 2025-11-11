using NouAssist.PersonalDevelopment.Goals.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.PersonalDevelopment.Goals.Permissions;

public class GoalsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(GoalsPermissions.GroupName, L("Permission:Goals"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<GoalsResource>(name);
    }
}
