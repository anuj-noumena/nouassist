using NouAssist.Comments.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Comments.Permissions;

public class CommentsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CommentsPermissions.GroupName, L("Permission:Comments"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CommentsResource>(name);
    }
}
