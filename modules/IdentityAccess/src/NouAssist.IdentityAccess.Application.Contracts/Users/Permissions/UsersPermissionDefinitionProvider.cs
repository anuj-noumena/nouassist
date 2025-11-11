using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.IdentityAccess.Users.Permissions
{
    public class UsersPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var usersGroup = context.AddGroup(UsersPermissions.GroupName, L("Permission:Users"));

            var usersPermission = usersGroup.AddPermission(
                UsersPermissions.Users.Default, 
                L("Permission:Users"));
            
            usersPermission.AddChild(
                UsersPermissions.Users.View, 
                L("Permission:Users.View"));
            
            usersPermission.AddChild(
                UsersPermissions.Users.Update, 
                L("Permission:Users.Update"));
            
            usersPermission.AddChild(
                UsersPermissions.Users.UpdateProfile, 
                L("Permission:Users.UpdateProfile"));
            
            usersPermission.AddChild(
                UsersPermissions.Users.ManageSettings, 
                L("Permission:Users.ManageSettings"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<NouAssistResource>(name);
        }
    }
}
