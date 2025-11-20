using NouAssist.RequirementsManagement.VersioningBaselining.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Permissions
{
    public class VersionControlPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            // Define a top-level group for the module
            var versionControlGroup = context.AddGroup(
                VersionControlPermissions.GroupName, 
                L("Permission:VersionControl"));

            // Versioning permissions
            var versioningPermission = versionControlGroup.AddPermission(
                VersionControlPermissions.Versioning.Default, 
                L("Permission:Versioning"));
                
            versioningPermission.AddChild(
                VersionControlPermissions.Versioning.Create, 
                L("Permission:Versioning.Create"));
                
            versioningPermission.AddChild(
                VersionControlPermissions.Versioning.View, 
                L("Permission:Versioning.View"));
                
            versioningPermission.AddChild(
                VersionControlPermissions.Versioning.Compare, 
                L("Permission:Versioning.Compare"));

            // Baselining permissions
            var baseliningPermission = versionControlGroup.AddPermission(
                VersionControlPermissions.Baselining.Default, 
                L("Permission:Baselining"));
                
            baseliningPermission.AddChild(
                VersionControlPermissions.Baselining.Create, 
                L("Permission:Baselining.Create"));
                
            baseliningPermission.AddChild(
                VersionControlPermissions.Baselining.Update, 
                L("Permission:Baselining.Update"));
                
            baseliningPermission.AddChild(
                VersionControlPermissions.Baselining.Delete, 
                L("Permission:Baselining.Delete"));
                
            baseliningPermission.AddChild(
                VersionControlPermissions.Baselining.View, 
                L("Permission:Baselining.View"));
                
            baseliningPermission.AddChild(
                VersionControlPermissions.Baselining.ManageStatus, 
                L("Permission:Baselining.ManageStatus"));

            // Branching permissions
            var branchingPermission = versionControlGroup.AddPermission(
                VersionControlPermissions.Branching.Default, 
                L("Permission:Branching"));
                
            branchingPermission.AddChild(
                VersionControlPermissions.Branching.Create, 
                L("Permission:Branching.Create"));
                
            branchingPermission.AddChild(
                VersionControlPermissions.Branching.Update, 
                L("Permission:Branching.Update"));
                
            branchingPermission.AddChild(
                VersionControlPermissions.Branching.Delete, 
                L("Permission:Branching.Delete"));
                
            branchingPermission.AddChild(
                VersionControlPermissions.Branching.View, 
                L("Permission:Branching.View"));
                
            branchingPermission.AddChild(
                VersionControlPermissions.Branching.Merge, 
                L("Permission:Branching.Merge"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<VersionControlResource>(name);
        }
    }
}
