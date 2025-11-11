using Volo.Abp.Reflection;

namespace NouAssist.LearningDevelopment.Permissions;

public class LearningDevelopmentPermissions
{
    public const string GroupName = "LearningDevelopment";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(LearningDevelopmentPermissions));
    }
}
