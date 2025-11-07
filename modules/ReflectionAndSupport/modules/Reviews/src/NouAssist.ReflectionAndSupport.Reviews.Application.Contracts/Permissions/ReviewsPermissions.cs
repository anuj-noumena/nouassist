using Volo.Abp.Reflection;

namespace NouAssist.ReflectionAndSupport.Reviews.Permissions;

public class ReviewsPermissions
{
    public const string GroupName = "Reviews";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ReviewsPermissions));
    }
}
