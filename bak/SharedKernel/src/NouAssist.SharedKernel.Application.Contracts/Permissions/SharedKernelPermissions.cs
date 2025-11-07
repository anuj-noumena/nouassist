using Volo.Abp.Reflection;

namespace NouAssist.SharedKernel.Permissions;

public class SharedKernelPermissions
{
    public const string GroupName = "SharedKernel";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(SharedKernelPermissions));
    }
}
