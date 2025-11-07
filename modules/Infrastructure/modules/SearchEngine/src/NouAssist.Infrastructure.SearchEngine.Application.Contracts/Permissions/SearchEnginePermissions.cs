using Volo.Abp.Reflection;

namespace NouAssist.Infrastructure.SearchEngine.Permissions;

public class SearchEnginePermissions
{
    public const string GroupName = "SearchEngine";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(SearchEnginePermissions));
    }
}
