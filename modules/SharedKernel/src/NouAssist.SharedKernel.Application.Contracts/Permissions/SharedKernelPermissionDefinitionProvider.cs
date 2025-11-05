using NouAssist.SharedKernel.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.SharedKernel.Permissions;

public class SharedKernelPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SharedKernelPermissions.GroupName, L("Permission:SharedKernel"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SharedKernelResource>(name);
    }
}
