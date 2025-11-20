using NouAssist.IntelligenceStrategy.StrategicPlanning.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Permissions
{
    /// <summary>
    /// Permission definition provider for Strategic Planning module
    /// </summary>
    public class StrategicPlanningPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var strategicPlanningGroup = context.AddGroup(
                StrategicPlanningPermissions.GroupName, 
                L("Permission:StrategicPlanning"));

            // Strategic Goals Permissions
            var strategicGoalsPermission = strategicPlanningGroup.AddPermission(
                StrategicPlanningPermissions.StrategicGoals.Default, 
                L("Permission:StrategicGoals"));

            strategicGoalsPermission.AddChild(
                StrategicPlanningPermissions.StrategicGoals.Create, 
                L("Permission:StrategicGoals.Create"));
            strategicGoalsPermission.AddChild(
                StrategicPlanningPermissions.StrategicGoals.Update, 
                L("Permission:StrategicGoals.Update"));
            strategicGoalsPermission.AddChild(
                StrategicPlanningPermissions.StrategicGoals.Delete, 
                L("Permission:StrategicGoals.Delete"));
            strategicGoalsPermission.AddChild(
                StrategicPlanningPermissions.StrategicGoals.View, 
                L("Permission:StrategicGoals.View"));
            strategicGoalsPermission.AddChild(
                StrategicPlanningPermissions.StrategicGoals.Approve, 
                L("Permission:StrategicGoals.Approve"));
            strategicGoalsPermission.AddChild(
                StrategicPlanningPermissions.StrategicGoals.Activate, 
                L("Permission:StrategicGoals.Activate"));
            strategicGoalsPermission.AddChild(
                StrategicPlanningPermissions.StrategicGoals.ManageTargets, 
                L("Permission:StrategicGoals.ManageTargets"));

            // Crisis Management Permissions
            var crisisManagementPermission = strategicPlanningGroup.AddPermission(
                StrategicPlanningPermissions.CrisisManagement.Default, 
                L("Permission:CrisisManagement"));

            crisisManagementPermission.AddChild(
                StrategicPlanningPermissions.CrisisManagement.Create, 
                L("Permission:CrisisManagement.Create"));
            crisisManagementPermission.AddChild(
                StrategicPlanningPermissions.CrisisManagement.Update, 
                L("Permission:CrisisManagement.Update"));
            crisisManagementPermission.AddChild(
                StrategicPlanningPermissions.CrisisManagement.Delete, 
                L("Permission:CrisisManagement.Delete"));
            crisisManagementPermission.AddChild(
                StrategicPlanningPermissions.CrisisManagement.View, 
                L("Permission:CrisisManagement.View"));
            crisisManagementPermission.AddChild(
                StrategicPlanningPermissions.CrisisManagement.Activate, 
                L("Permission:CrisisManagement.Activate"));
            crisisManagementPermission.AddChild(
                StrategicPlanningPermissions.CrisisManagement.ManageProtocols, 
                L("Permission:CrisisManagement.ManageProtocols"));

            // Sustainable Growth Permissions
            var sustainableGrowthPermission = strategicPlanningGroup.AddPermission(
                StrategicPlanningPermissions.SustainableGrowth.Default, 
                L("Permission:SustainableGrowth"));

            sustainableGrowthPermission.AddChild(
                StrategicPlanningPermissions.SustainableGrowth.Create, 
                L("Permission:SustainableGrowth.Create"));
            sustainableGrowthPermission.AddChild(
                StrategicPlanningPermissions.SustainableGrowth.Update, 
                L("Permission:SustainableGrowth.Update"));
            sustainableGrowthPermission.AddChild(
                StrategicPlanningPermissions.SustainableGrowth.Delete, 
                L("Permission:SustainableGrowth.Delete"));
            sustainableGrowthPermission.AddChild(
                StrategicPlanningPermissions.SustainableGrowth.View, 
                L("Permission:SustainableGrowth.View"));
            sustainableGrowthPermission.AddChild(
                StrategicPlanningPermissions.SustainableGrowth.ManageEsgGoals, 
                L("Permission:SustainableGrowth.ManageEsgGoals"));
            sustainableGrowthPermission.AddChild(
                StrategicPlanningPermissions.SustainableGrowth.RecordMeasurements, 
                L("Permission:SustainableGrowth.RecordMeasurements"));

            // Integrated Planning Permissions
            var integratedPlanningPermission = strategicPlanningGroup.AddPermission(
                StrategicPlanningPermissions.IntegratedPlanning.Default, 
                L("Permission:IntegratedPlanning"));

            integratedPlanningPermission.AddChild(
                StrategicPlanningPermissions.IntegratedPlanning.Create, 
                L("Permission:IntegratedPlanning.Create"));
            integratedPlanningPermission.AddChild(
                StrategicPlanningPermissions.IntegratedPlanning.Update, 
                L("Permission:IntegratedPlanning.Update"));
            integratedPlanningPermission.AddChild(
                StrategicPlanningPermissions.IntegratedPlanning.Delete, 
                L("Permission:IntegratedPlanning.Delete"));
            integratedPlanningPermission.AddChild(
                StrategicPlanningPermissions.IntegratedPlanning.View, 
                L("Permission:IntegratedPlanning.View"));
            integratedPlanningPermission.AddChild(
                StrategicPlanningPermissions.IntegratedPlanning.ManageTimeline, 
                L("Permission:IntegratedPlanning.ManageTimeline"));
            integratedPlanningPermission.AddChild(
                StrategicPlanningPermissions.IntegratedPlanning.BackwardPlanning, 
                L("Permission:IntegratedPlanning.BackwardPlanning"));

            // Opportunity Management Permissions
            var opportunityManagementPermission = strategicPlanningGroup.AddPermission(
                StrategicPlanningPermissions.OpportunityManagement.Default, 
                L("Permission:OpportunityManagement"));

            opportunityManagementPermission.AddChild(
                StrategicPlanningPermissions.OpportunityManagement.Create, 
                L("Permission:OpportunityManagement.Create"));
            opportunityManagementPermission.AddChild(
                StrategicPlanningPermissions.OpportunityManagement.Update, 
                L("Permission:OpportunityManagement.Update"));
            opportunityManagementPermission.AddChild(
                StrategicPlanningPermissions.OpportunityManagement.Delete, 
                L("Permission:OpportunityManagement.Delete"));
            opportunityManagementPermission.AddChild(
                StrategicPlanningPermissions.OpportunityManagement.View, 
                L("Permission:OpportunityManagement.View"));
            opportunityManagementPermission.AddChild(
                StrategicPlanningPermissions.OpportunityManagement.ManageFilters, 
                L("Permission:OpportunityManagement.ManageFilters"));
            opportunityManagementPermission.AddChild(
                StrategicPlanningPermissions.OpportunityManagement.EvaluateOpportunities, 
                L("Permission:OpportunityManagement.EvaluateOpportunities"));

            // Transition Planning Permissions
            var transitionPlanningPermission = strategicPlanningGroup.AddPermission(
                StrategicPlanningPermissions.TransitionPlanning.Default, 
                L("Permission:TransitionPlanning"));

            transitionPlanningPermission.AddChild(
                StrategicPlanningPermissions.TransitionPlanning.Create, 
                L("Permission:TransitionPlanning.Create"));
            transitionPlanningPermission.AddChild(
                StrategicPlanningPermissions.TransitionPlanning.Update, 
                L("Permission:TransitionPlanning.Update"));
            transitionPlanningPermission.AddChild(
                StrategicPlanningPermissions.TransitionPlanning.Delete, 
                L("Permission:TransitionPlanning.Delete"));
            transitionPlanningPermission.AddChild(
                StrategicPlanningPermissions.TransitionPlanning.View, 
                L("Permission:TransitionPlanning.View"));
            transitionPlanningPermission.AddChild(
                StrategicPlanningPermissions.TransitionPlanning.ManageCapabilities, 
                L("Permission:TransitionPlanning.ManageCapabilities"));
            transitionPlanningPermission.AddChild(
                StrategicPlanningPermissions.TransitionPlanning.UpdateProgress, 
                L("Permission:TransitionPlanning.UpdateProgress"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<StrategicPlanningResource>(name);
        }
    }
}
