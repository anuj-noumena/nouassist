using NouAssist.WorkflowOrchestration.GTDSystem.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.WorkflowOrchestration.GTDSystem.Permissions;

public class GTDSystemPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(GTDSystemPermissions.GroupName, L("Permission:GTDSystem"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<GTDSystemResource>(name);
    }
}
