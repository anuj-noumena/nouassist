using NouAssist.CollaborationHub.DiscussionThreads.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CollaborationHub.DiscussionThreads.Permissions;

public class DiscussionThreadsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(DiscussionThreadsPermissions.GroupName, L("Permission:DiscussionThreads"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DiscussionThreadsResource>(name);
    }
}
