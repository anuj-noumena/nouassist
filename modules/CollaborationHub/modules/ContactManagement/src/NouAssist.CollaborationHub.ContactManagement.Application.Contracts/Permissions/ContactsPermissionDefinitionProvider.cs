using NouAssist.CollaborationHub.ContactManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CollaborationHub.ContactManagement.Permissions;

public class ContactsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ContactsPermissions.GroupName, L("Permission:Contacts"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ContactManagementResource>(name);
    }
}
