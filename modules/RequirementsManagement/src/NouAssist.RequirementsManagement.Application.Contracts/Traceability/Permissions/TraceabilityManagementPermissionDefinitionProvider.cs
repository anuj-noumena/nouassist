using NouAssist.RequirementsManagement.Traceability.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.RequirementsManagement.Traceability.Permissions
{
    /// <summary>
    /// Permission definition provider for TraceabilityManagement module
    /// </summary>
    public class TraceabilityManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var traceabilityGroup = context.AddGroup(
                TraceabilityManagementPermissions.GroupName,
                L("Permission:TraceabilityManagement"));
            
            // TraceLinks permissions
            var traceLinksPermission = traceabilityGroup.AddPermission(
                TraceabilityManagementPermissions.TraceLinks.Default,
                L("Permission:TraceLinks"));
                
            traceLinksPermission.AddChild(
                TraceabilityManagementPermissions.TraceLinks.Create,
                L("Permission:TraceLinks.Create"));
                
            traceLinksPermission.AddChild(
                TraceabilityManagementPermissions.TraceLinks.Update,
                L("Permission:TraceLinks.Update"));
                
            traceLinksPermission.AddChild(
                TraceabilityManagementPermissions.TraceLinks.Delete,
                L("Permission:TraceLinks.Delete"));
                
            traceLinksPermission.AddChild(
                TraceabilityManagementPermissions.TraceLinks.View,
                L("Permission:TraceLinks.View"));
                
            traceLinksPermission.AddChild(
                TraceabilityManagementPermissions.TraceLinks.Verify,
                L("Permission:TraceLinks.Verify"));
            
            // TraceMatrices permissions
            var matricesPermission = traceabilityGroup.AddPermission(
                TraceabilityManagementPermissions.TraceMatrices.Default,
                L("Permission:TraceMatrices"));
                
            matricesPermission.AddChild(
                TraceabilityManagementPermissions.TraceMatrices.Generate,
                L("Permission:TraceMatrices.Generate"));
                
            matricesPermission.AddChild(
                TraceabilityManagementPermissions.TraceMatrices.CheckCoverage,
                L("Permission:TraceMatrices.CheckCoverage"));
            
            // ImpactAnalysis permissions
            var impactAnalysisPermission = traceabilityGroup.AddPermission(
                TraceabilityManagementPermissions.ImpactAnalysis.Default,
                L("Permission:ImpactAnalysis"));
                
            impactAnalysisPermission.AddChild(
                TraceabilityManagementPermissions.ImpactAnalysis.Analyze,
                L("Permission:ImpactAnalysis.Analyze"));
            
            // ArtifactRegistry permissions
            var artifactRegistryPermission = traceabilityGroup.AddPermission(
                TraceabilityManagementPermissions.ArtifactRegistry.Default,
                L("Permission:ArtifactRegistry"));
                
            artifactRegistryPermission.AddChild(
                TraceabilityManagementPermissions.ArtifactRegistry.Register,
                L("Permission:ArtifactRegistry.Register"));
                
            artifactRegistryPermission.AddChild(
                TraceabilityManagementPermissions.ArtifactRegistry.Verify,
                L("Permission:ArtifactRegistry.Verify"));
        }
        
        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<TraceabilityManagementResource>(name);
        }
    }
}
