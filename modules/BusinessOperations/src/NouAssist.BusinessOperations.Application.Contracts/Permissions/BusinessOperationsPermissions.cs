using Volo.Abp.Reflection;

namespace NouAssist.BusinessOperations.Permissions;

public class BusinessOperationsPermissions
{
    public const string GroupName = "BusinessOperations";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(BusinessOperationsPermissions));
    }
}
