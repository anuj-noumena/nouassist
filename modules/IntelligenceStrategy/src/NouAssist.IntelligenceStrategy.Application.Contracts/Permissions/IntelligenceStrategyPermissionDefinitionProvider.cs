using NouAssist.IntelligenceStrategy.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.IntelligenceStrategy.Permissions;

public class IntelligenceStrategyPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(IntelligenceStrategyPermissions.GroupName, L("Permission:IntelligenceStrategy"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IntelligenceStrategyResource>(name);
    }
}
