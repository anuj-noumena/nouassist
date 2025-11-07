using NouAssist.ReflectionAndSupport.Reviews.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.ReflectionAndSupport.Reviews.Permissions
{
    public class ReviewsPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var reviewsGroup = context.AddGroup(ReviewsPermissions.GroupName, L("Permission:Reviews"));

            var reviewsPermission = reviewsGroup.AddPermission(
                ReviewsPermissions.Reviews.Default, L("Permission:Reviews"));
            reviewsPermission.AddChild(ReviewsPermissions.Reviews.Create, L("Permission:Reviews.Create"));
            reviewsPermission.AddChild(ReviewsPermissions.Reviews.Edit, L("Permission:Reviews.Edit"));
            reviewsPermission.AddChild(ReviewsPermissions.Reviews.Delete, L("Permission:Reviews.Delete"));
            reviewsPermission.AddChild(ReviewsPermissions.Reviews.Complete, L("Permission:Reviews.Complete"));
            reviewsPermission.AddChild(ReviewsPermissions.Reviews.ViewOwn, L("Permission:Reviews.ViewOwn"));

            var templatesPermission = reviewsGroup.AddPermission(
                ReviewsPermissions.Templates.Default, L("Permission:Templates"));
            templatesPermission.AddChild(ReviewsPermissions.Templates.Create, L("Permission:Templates.Create"));
            templatesPermission.AddChild(ReviewsPermissions.Templates.Edit, L("Permission:Templates.Edit"));
            templatesPermission.AddChild(ReviewsPermissions.Templates.Delete, L("Permission:Templates.Delete"));
            templatesPermission.AddChild(ReviewsPermissions.Templates.ViewSystem, L("Permission:Templates.ViewSystem"));

            var analyticsPermission = reviewsGroup.AddPermission(
                ReviewsPermissions.Analytics.Default, L("Permission:Analytics"));
            analyticsPermission.AddChild(ReviewsPermissions.Analytics.ViewOwn, L("Permission:Analytics.ViewOwn"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ReviewsResource>(name);
        }
    }
}
