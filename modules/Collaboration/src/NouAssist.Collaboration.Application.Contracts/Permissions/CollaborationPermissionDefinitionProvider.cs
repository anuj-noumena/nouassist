using NouAssist.Collaboration.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Collaboration.Permissions;

public class CollaborationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CollaborationPermissions.GroupName, L("Permission:Collaboration"));

        // Comments Permissions
        var commentsGroup = context.AddGroup(
            CollaborationPermissions.Comments.GroupName,
            L("Permission:Comments"));

        var commentsPermission = commentsGroup.AddPermission(
            CollaborationPermissions.Comments.Comment.Default,
            L("Permission:Comments"));

        commentsPermission.AddChild(
            CollaborationPermissions.Comments.Comment.Create,
            L("Permission:Comments.Create"));

        commentsPermission.AddChild(
            CollaborationPermissions.Comments.Comment.Edit,
            L("Permission:Comments.Edit"));

        commentsPermission.AddChild(
            CollaborationPermissions.Comments.Comment.Delete,
            L("Permission:Comments.Delete"));

        commentsPermission.AddChild(
            CollaborationPermissions.Comments.Comment.Pin,
            L("Permission:Comments.Pin"));

        commentsPermission.AddChild(
            CollaborationPermissions.Comments.Comment.React,
            L("Permission:Comments.React"));

        commentsPermission.AddChild(
            CollaborationPermissions.Comments.Comment.Moderate,
            L("Permission:Comments.Moderate"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CollaborationResource>(name);
    }
}
