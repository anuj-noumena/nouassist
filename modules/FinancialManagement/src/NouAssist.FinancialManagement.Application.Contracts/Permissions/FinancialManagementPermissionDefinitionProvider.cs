using NouAssist.FinancialManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.FinancialManagement.Permissions;

public class FinancialManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(FinancialManagementPermissions.GroupName, L("Permission:FinancialManagement"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FinancialManagementResource>(name);
    }
}
