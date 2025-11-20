using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Permissions
{
    /// <summary>
    /// Permission definition provider for Personal Productivity Intelligence module
    /// </summary>
    public class PersonalProductivityIntelligencePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var productivityGroup = context.AddGroup(
                PersonalProductivityIntelligencePermissions.GroupName,
                L("Permission:PersonalProductivityIntelligence"));
            
            // User Profile Permissions
            var userProfileGroup = productivityGroup.AddPermission(
                PersonalProductivityIntelligencePermissions.UserProfile.Default,
                L("Permission:UserProductivityProfile"));
            
            userProfileGroup.AddChild(
                PersonalProductivityIntelligencePermissions.UserProfile.View,
                L("Permission:UserProductivityProfile.View"));
            
            userProfileGroup.AddChild(
                PersonalProductivityIntelligencePermissions.UserProfile.UpdateCognitiveLoad,
                L("Permission:UserProductivityProfile.UpdateCognitiveLoad"));
            
            userProfileGroup.AddChild(
                PersonalProductivityIntelligencePermissions.UserProfile.RecordTaskCompletion,
                L("Permission:UserProductivityProfile.RecordTaskCompletion"));
            
            userProfileGroup.AddChild(
                PersonalProductivityIntelligencePermissions.UserProfile.UpdateContext,
                L("Permission:UserProductivityProfile.UpdateContext"));
            
            userProfileGroup.AddChild(
                PersonalProductivityIntelligencePermissions.UserProfile.SetPreference,
                L("Permission:UserProductivityProfile.SetPreference"));
            
            userProfileGroup.AddChild(
                PersonalProductivityIntelligencePermissions.UserProfile.AnalyzeWorkPattern,
                L("Permission:UserProductivityProfile.AnalyzeWorkPattern"));
            
            // Prediction Permissions
            var predictionGroup = productivityGroup.AddPermission(
                PersonalProductivityIntelligencePermissions.Prediction.Default,
                L("Permission:PredictionManagement"));
            
            predictionGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Prediction.View,
                L("Permission:PredictionManagement.View"));
            
            predictionGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Prediction.Create,
                L("Permission:PredictionManagement.Create"));
            
            predictionGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Prediction.RecordOutcome,
                L("Permission:PredictionManagement.RecordOutcome"));
            
            // Recommendation Permissions
            var recommendationGroup = productivityGroup.AddPermission(
                PersonalProductivityIntelligencePermissions.Recommendation.Default,
                L("Permission:RecommendationManagement"));
            
            recommendationGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Recommendation.View,
                L("Permission:RecommendationManagement.View"));
            
            recommendationGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Recommendation.Generate,
                L("Permission:RecommendationManagement.Generate"));
            
            recommendationGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Recommendation.Mark,
                L("Permission:RecommendationManagement.Mark"));
            
            recommendationGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Recommendation.Rate,
                L("Permission:RecommendationManagement.Rate"));
            
            // Automation Permissions
            var automationGroup = productivityGroup.AddPermission(
                PersonalProductivityIntelligencePermissions.Automation.Default,
                L("Permission:AutomationManagement"));
            
            automationGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Automation.View,
                L("Permission:AutomationManagement.View"));
            
            automationGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Automation.Create,
                L("Permission:AutomationManagement.Create"));
            
            automationGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Automation.Edit,
                L("Permission:AutomationManagement.Edit"));
            
            automationGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Automation.Delete,
                L("Permission:AutomationManagement.Delete"));
            
            automationGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Automation.Execute,
                L("Permission:AutomationManagement.Execute"));
            
            // Performance Permissions
            var performanceGroup = productivityGroup.AddPermission(
                PersonalProductivityIntelligencePermissions.Performance.Default,
                L("Permission:PerformanceTracking"));
            
            performanceGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Performance.View,
                L("Permission:PerformanceTracking.View"));
            
            performanceGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Performance.Create,
                L("Permission:PerformanceTracking.Create"));
            
            performanceGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Performance.GenerateInsights,
                L("Permission:PerformanceTracking.GenerateInsights"));
            
            performanceGroup.AddChild(
                PersonalProductivityIntelligencePermissions.Performance.ViewTrends,
                L("Permission:PerformanceTracking.ViewTrends"));
        }
        
        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PersonalProductivityIntelligenceResource>(name);
        }
    }
}
