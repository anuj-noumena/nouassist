using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Permissions
{
    public class KnowledgeInnovationPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var knowledgeInnovationGroup = context.AddGroup(
                KnowledgeInnovationPermissions.GroupName,
                L("Permission:KnowledgeInnovation"));

            // Knowledge Asset permissions
            var knowledgeAssetPermission = knowledgeInnovationGroup.AddPermission(
                KnowledgeInnovationPermissions.KnowledgeAsset.Default,
                L("Permission:KnowledgeAsset"));
            
            knowledgeAssetPermission.AddChild(
                KnowledgeInnovationPermissions.KnowledgeAsset.Create,
                L("Permission:KnowledgeAsset.Create"));
            
            knowledgeAssetPermission.AddChild(
                KnowledgeInnovationPermissions.KnowledgeAsset.Update,
                L("Permission:KnowledgeAsset.Edit"));
            
            knowledgeAssetPermission.AddChild(
                KnowledgeInnovationPermissions.KnowledgeAsset.Delete,
                L("Permission:KnowledgeAsset.Delete"));
            
            knowledgeAssetPermission.AddChild(
                KnowledgeInnovationPermissions.KnowledgeAsset.View,
                L("Permission:KnowledgeAsset.View"));

            // Learning Resource permissions
            var learningResourcePermission = knowledgeInnovationGroup.AddPermission(
                KnowledgeInnovationPermissions.LearningResource.Default,
                L("Permission:LearningResource"));
            
            learningResourcePermission.AddChild(
                KnowledgeInnovationPermissions.LearningResource.Create,
                L("Permission:LearningResource.Create"));
            
            learningResourcePermission.AddChild(
                KnowledgeInnovationPermissions.LearningResource.Update,
                L("Permission:LearningResource.Edit"));
            
            learningResourcePermission.AddChild(
                KnowledgeInnovationPermissions.LearningResource.Delete,
                L("Permission:LearningResource.Delete"));
            
            learningResourcePermission.AddChild(
                KnowledgeInnovationPermissions.LearningResource.View,
                L("Permission:LearningResource.View"));
            
            learningResourcePermission.AddChild(
                KnowledgeInnovationPermissions.LearningResource.Publish,
                L("Permission:LearningResource.Publish"));
            
            learningResourcePermission.AddChild(
                KnowledgeInnovationPermissions.LearningResource.Feature,
                L("Permission:LearningResource.Feature"));

            // Idea permissions
            var ideaPermission = knowledgeInnovationGroup.AddPermission(
                KnowledgeInnovationPermissions.Idea.Default,
                L("Permission:Idea"));
            
            ideaPermission.AddChild(
                KnowledgeInnovationPermissions.Idea.Create,
                L("Permission:Idea.Create"));
            
            ideaPermission.AddChild(
                KnowledgeInnovationPermissions.Idea.Update,
                L("Permission:Idea.Edit"));
            
            ideaPermission.AddChild(
                KnowledgeInnovationPermissions.Idea.Delete,
                L("Permission:Idea.Delete"));
            
            ideaPermission.AddChild(
                KnowledgeInnovationPermissions.Idea.View,
                L("Permission:Idea.View"));
            
            ideaPermission.AddChild(
                KnowledgeInnovationPermissions.Idea.Evaluate,
                L("Permission:Idea.Evaluate"));
            
            ideaPermission.AddChild(
                KnowledgeInnovationPermissions.Idea.ChangeStatus,
                L("Permission:Idea.ChangeStatus"));

            // Decision Record permissions
            var decisionRecordPermission = knowledgeInnovationGroup.AddPermission(
                KnowledgeInnovationPermissions.DecisionRecord.Default,
                L("Permission:DecisionRecord"));
            
            decisionRecordPermission.AddChild(
                KnowledgeInnovationPermissions.DecisionRecord.Create,
                L("Permission:DecisionRecord.Create"));
            
            decisionRecordPermission.AddChild(
                KnowledgeInnovationPermissions.DecisionRecord.Update,
                L("Permission:DecisionRecord.Edit"));
            
            decisionRecordPermission.AddChild(
                KnowledgeInnovationPermissions.DecisionRecord.Delete,
                L("Permission:DecisionRecord.Delete"));
            
            decisionRecordPermission.AddChild(
                KnowledgeInnovationPermissions.DecisionRecord.View,
                L("Permission:DecisionRecord.View"));
            
            decisionRecordPermission.AddChild(
                KnowledgeInnovationPermissions.DecisionRecord.MakeDecision,
                L("Permission:DecisionRecord.MakeDecision"));
            
            decisionRecordPermission.AddChild(
                KnowledgeInnovationPermissions.DecisionRecord.Implement,
                L("Permission:DecisionRecord.Implement"));
            
            decisionRecordPermission.AddChild(
                KnowledgeInnovationPermissions.DecisionRecord.Evaluate,
                L("Permission:DecisionRecord.Evaluate"));

            // Mentor Profile permissions
            var mentorProfilePermission = knowledgeInnovationGroup.AddPermission(
                KnowledgeInnovationPermissions.MentorProfile.Default,
                L("Permission:MentorProfile"));
            
            mentorProfilePermission.AddChild(
                KnowledgeInnovationPermissions.MentorProfile.Create,
                L("Permission:MentorProfile.Create"));
            
            mentorProfilePermission.AddChild(
                KnowledgeInnovationPermissions.MentorProfile.Update,
                L("Permission:MentorProfile.Edit"));
            
            mentorProfilePermission.AddChild(
                KnowledgeInnovationPermissions.MentorProfile.Delete,
                L("Permission:MentorProfile.Delete"));
            
            mentorProfilePermission.AddChild(
                KnowledgeInnovationPermissions.MentorProfile.View,
                L("Permission:MentorProfile.View"));
            
            mentorProfilePermission.AddChild(
                KnowledgeInnovationPermissions.MentorProfile.ManageRelationships,
                L("Permission:MentorProfile.ManageRelationships"));
            
            mentorProfilePermission.AddChild(
                KnowledgeInnovationPermissions.MentorProfile.AddAdvice,
                L("Permission:MentorProfile.AddAdvice"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<KnowledgeInnovationResource>(name);
        }
    }
}
