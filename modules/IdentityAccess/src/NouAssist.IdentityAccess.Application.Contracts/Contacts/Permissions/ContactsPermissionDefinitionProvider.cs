using NouAssist.IdentityAccess.Contacts.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.IdentityAccess.Contacts.Permissions;

public class ContactsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ContactsPermissions.GroupName, L("Permission:Contacts"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ContactsResource>(name);
    }
}
