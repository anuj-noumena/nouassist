using Volo.Abp.Reflection;

namespace NouAssist.FinancialManagement.Permissions;

public class FinancialManagementPermissions
{
    public const string GroupName = "FinancialManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(FinancialManagementPermissions));
    }
}
