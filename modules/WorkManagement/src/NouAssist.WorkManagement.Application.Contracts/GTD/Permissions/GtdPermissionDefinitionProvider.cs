using NouAssist.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.GTD.Permissions
{
    public class GtdPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var gtdGroup = context.AddGroup(GtdPermissions.GroupName, L("Permission:GTD"));

            // Horizons
            var horizonsPermission = gtdGroup.AddPermission(
                GtdPermissions.Horizons.Default, 
                L("Permission:GTD.Horizons"));
            horizonsPermission.AddChild(GtdPermissions.Horizons.Create, L("Permission:Create"));
            horizonsPermission.AddChild(GtdPermissions.Horizons.Update, L("Permission:Update"));
            horizonsPermission.AddChild(GtdPermissions.Horizons.Delete, L("Permission:Delete"));
            horizonsPermission.AddChild(GtdPermissions.Horizons.Review, L("Permission:Review"));
            horizonsPermission.AddChild(GtdPermissions.Horizons.Publish, L("Permission:Publish"));

            // Ticklers
            var ticklersPermission = gtdGroup.AddPermission(
                GtdPermissions.Ticklers.Default, 
                L("Permission:GTD.Ticklers"));
            ticklersPermission.AddChild(GtdPermissions.Ticklers.Create, L("Permission:Create"));
            ticklersPermission.AddChild(GtdPermissions.Ticklers.Update, L("Permission:Update"));
            ticklersPermission.AddChild(GtdPermissions.Ticklers.Delete, L("Permission:Delete"));
            ticklersPermission.AddChild(GtdPermissions.Ticklers.Surface, L("Permission:Surface"));
            ticklersPermission.AddChild(GtdPermissions.Ticklers.Process, L("Permission:Process"));

            // SomedayMaybe
            var somedayMaybePermission = gtdGroup.AddPermission(
                GtdPermissions.SomedayMaybe.Default, 
                L("Permission:GTD.SomedayMaybe"));
            somedayMaybePermission.AddChild(GtdPermissions.SomedayMaybe.Create, L("Permission:Create"));
            somedayMaybePermission.AddChild(GtdPermissions.SomedayMaybe.Update, L("Permission:Update"));
            somedayMaybePermission.AddChild(GtdPermissions.SomedayMaybe.Delete, L("Permission:Delete"));
            somedayMaybePermission.AddChild(GtdPermissions.SomedayMaybe.Promote, L("Permission:Promote"));
            somedayMaybePermission.AddChild(GtdPermissions.SomedayMaybe.Review, L("Permission:Review"));

            // WaitingFor
            var waitingForPermission = gtdGroup.AddPermission(
                GtdPermissions.WaitingFor.Default, 
                L("Permission:GTD.WaitingFor"));
            waitingForPermission.AddChild(GtdPermissions.WaitingFor.Create, L("Permission:Create"));
            waitingForPermission.AddChild(GtdPermissions.WaitingFor.Update, L("Permission:Update"));
            waitingForPermission.AddChild(GtdPermissions.WaitingFor.Delete, L("Permission:Delete"));
            waitingForPermission.AddChild(GtdPermissions.WaitingFor.Resolve, L("Permission:Resolve"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<NouAssistResource>(name);
        }
    }
}
