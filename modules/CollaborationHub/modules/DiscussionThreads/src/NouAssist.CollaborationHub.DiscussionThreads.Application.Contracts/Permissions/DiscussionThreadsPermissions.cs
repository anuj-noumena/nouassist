using Volo.Abp.Reflection;

namespace NouAssist.CollaborationHub.DiscussionThreads.Permissions;

public class DiscussionThreadsPermissions
{
    public const string GroupName = "DiscussionThreads";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(DiscussionThreadsPermissions));
    }
}
