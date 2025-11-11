using NouAssist.LearningDevelopment.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.LearningDevelopment.Permissions;

public class LearningDevelopmentPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(LearningDevelopmentPermissions.GroupName, L("Permission:LearningDevelopment"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<LearningDevelopmentResource>(name);
    }
}
