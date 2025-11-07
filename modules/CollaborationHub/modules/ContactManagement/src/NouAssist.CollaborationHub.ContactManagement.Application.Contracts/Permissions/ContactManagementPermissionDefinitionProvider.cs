using NouAssist.CollaborationHub.ContactManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CollaborationHub.ContactManagement.Permissions;

public class ContactManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ContactManagementPermissions.GroupName, L("Permission:ContactManagement"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ContactManagementResource>(name);
    }
}
