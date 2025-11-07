using Volo.Abp.Reflection;

namespace NouAssist.ReflectionAndSupport.CustomFields.Permissions;

public class CustomFieldsPermissions
{
    public const string GroupName = "CustomFields";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CustomFieldsPermissions));
    }
}
