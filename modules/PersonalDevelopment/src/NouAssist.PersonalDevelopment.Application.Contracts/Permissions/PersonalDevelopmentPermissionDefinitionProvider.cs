using NouAssist.PersonalDevelopment.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.PersonalDevelopment.Permissions;

public class PersonalDevelopmentPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(PersonalDevelopmentPermissions.GroupName, L("Permission:PersonalDevelopment"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PersonalDevelopmentResource>(name);
    }
}
