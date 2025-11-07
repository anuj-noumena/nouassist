using NouAssist.ReflectionAndSupport.Reviews.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.ReflectionAndSupport.Reviews.Permissions;

public class ReviewsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ReviewsPermissions.GroupName, L("Permission:Reviews"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ReviewsResource>(name);
    }
}
