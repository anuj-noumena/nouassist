using NouAssist.BusinessOperations.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.BusinessOperations.Permissions;

public class BusinessOperationsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BusinessOperationsPermissions.GroupName, L("Permission:BusinessOperations"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BusinessOperationsResource>(name);
    }
}
