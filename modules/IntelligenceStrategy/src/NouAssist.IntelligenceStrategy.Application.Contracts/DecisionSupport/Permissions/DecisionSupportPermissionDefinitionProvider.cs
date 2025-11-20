using NouAssist.IntelligenceStrategy.DecisionSupport.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Permissions
{
    /// <summary>
    /// Permission definition provider for Decision Support module
    /// </summary>
    public class DecisionSupportPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        /// <inheritdoc/>
        public override void Define(IPermissionDefinitionContext context)
        {
            var decisionSupportGroup = context.AddGroup(
                DecisionSupportPermissions.GroupName,
                L("Permission:DecisionSupport"));

            // Strategic Initiative permissions
            var initiativePermission = decisionSupportGroup.AddPermission(
                DecisionSupportPermissions.StrategicInitiative.Default,
                L("Permission:StrategicInitiative"));
                
            initiativePermission.AddChild(
                DecisionSupportPermissions.StrategicInitiative.Create,
                L("Permission:StrategicInitiative.Create"));
                
            initiativePermission.AddChild(
                DecisionSupportPermissions.StrategicInitiative.View,
                L("Permission:StrategicInitiative.View"));
                
            initiativePermission.AddChild(
                DecisionSupportPermissions.StrategicInitiative.Update,
                L("Permission:StrategicInitiative.Update"));
                
            initiativePermission.AddChild(
                DecisionSupportPermissions.StrategicInitiative.Delete,
                L("Permission:StrategicInitiative.Delete"));
                
            initiativePermission.AddChild(
                DecisionSupportPermissions.StrategicInitiative.Approve,
                L("Permission:StrategicInitiative.Approve"));
                
            initiativePermission.AddChild(
                DecisionSupportPermissions.StrategicInitiative.Reject,
                L("Permission:StrategicInitiative.Reject"));

            // Decision Model permissions
            var decisionModelPermission = decisionSupportGroup.AddPermission(
                DecisionSupportPermissions.DecisionModel.Default,
                L("Permission:DecisionModel"));
                
            decisionModelPermission.AddChild(
                DecisionSupportPermissions.DecisionModel.Create,
                L("Permission:DecisionModel.Create"));
                
            decisionModelPermission.AddChild(
                DecisionSupportPermissions.DecisionModel.View,
                L("Permission:DecisionModel.View"));
                
            decisionModelPermission.AddChild(
                DecisionSupportPermissions.DecisionModel.Update,
                L("Permission:DecisionModel.Update"));
                
            decisionModelPermission.AddChild(
                DecisionSupportPermissions.DecisionModel.Delete,
                L("Permission:DecisionModel.Delete"));
                
            decisionModelPermission.AddChild(
                DecisionSupportPermissions.DecisionModel.Finalize,
                L("Permission:DecisionModel.Finalize"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DecisionSupportResource>(name);
        }
    }
}
