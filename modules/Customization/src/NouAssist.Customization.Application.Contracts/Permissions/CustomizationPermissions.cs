using Volo.Abp.Reflection;

namespace NouAssist.Customization.Permissions;

public class CustomizationPermissions
{
    public const string GroupName = "Customization";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CustomizationPermissions));
    }
}
