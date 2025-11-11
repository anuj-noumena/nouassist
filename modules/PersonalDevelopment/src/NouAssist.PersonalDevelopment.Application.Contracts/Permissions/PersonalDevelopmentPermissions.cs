using Volo.Abp.Reflection;

namespace NouAssist.PersonalDevelopment.Permissions;

public class PersonalDevelopmentPermissions
{
    public const string GroupName = "PersonalDevelopment";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(PersonalDevelopmentPermissions));
    }
}
