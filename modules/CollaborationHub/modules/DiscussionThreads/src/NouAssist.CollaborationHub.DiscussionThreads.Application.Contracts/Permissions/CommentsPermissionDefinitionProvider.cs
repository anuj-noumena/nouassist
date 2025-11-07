using NouAssist.CollaborationHub.DiscussionThreads.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CollaborationHub.DiscussionThreads.Permissions;

public class CommentsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CommentsPermissions.GroupName, L("Permission:Comments"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DiscussionThreadsResource>(name);
    }
}
