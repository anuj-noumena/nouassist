using NouAssist.Learning.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.LearningDevelopment.Learning.Permissions
{
    public class LearningPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var learningGroup = context.AddGroup(
                LearningPermissions.GroupName,
                L("Permission:Learning"));

            // Topics
            var topicsPermission = learningGroup.AddPermission(
                LearningPermissions.Topics.Default,
                L("Permission:Topics"));
            topicsPermission.AddChild(LearningPermissions.Topics.Create, L("Permission:Topics.Create"));
            topicsPermission.AddChild(LearningPermissions.Topics.Update, L("Permission:Topics.Update"));
            topicsPermission.AddChild(LearningPermissions.Topics.Delete, L("Permission:Topics.Delete"));
            topicsPermission.AddChild(LearningPermissions.Topics.ManageChildren, L("Permission:Topics.ManageChildren"));

            // Learning Sessions
            var sessionsPermission = learningGroup.AddPermission(
                LearningPermissions.LearningSessions.Default,
                L("Permission:LearningSessions"));
            sessionsPermission.AddChild(LearningPermissions.LearningSessions.Start, L("Permission:LearningSessions.Start"));
            sessionsPermission.AddChild(LearningPermissions.LearningSessions.Advance, L("Permission:LearningSessions.Advance"));
            sessionsPermission.AddChild(LearningPermissions.LearningSessions.Pause, L("Permission:LearningSessions.Pause"));
            sessionsPermission.AddChild(LearningPermissions.LearningSessions.Resume, L("Permission:LearningSessions.Resume"));
            sessionsPermission.AddChild(LearningPermissions.LearningSessions.RecordData, L("Permission:LearningSessions.RecordData"));

            // Learning Notes
            var notesPermission = learningGroup.AddPermission(
                LearningPermissions.LearningNotes.Default,
                L("Permission:LearningNotes"));
            notesPermission.AddChild(LearningPermissions.LearningNotes.Create, L("Permission:LearningNotes.Create"));
            notesPermission.AddChild(LearningPermissions.LearningNotes.Update, L("Permission:LearningNotes.Update"));
            notesPermission.AddChild(LearningPermissions.LearningNotes.Delete, L("Permission:LearningNotes.Delete"));

            // Mindmaps
            var mindmapsPermission = learningGroup.AddPermission(
                LearningPermissions.Mindmaps.Default,
                L("Permission:Mindmaps"));
            mindmapsPermission.AddChild(LearningPermissions.Mindmaps.Create, L("Permission:Mindmaps.Create"));
            mindmapsPermission.AddChild(LearningPermissions.Mindmaps.Update, L("Permission:Mindmaps.Update"));
            mindmapsPermission.AddChild(LearningPermissions.Mindmaps.Delete, L("Permission:Mindmaps.Delete"));

            // Book Reading Histories
            var bookHistoriesPermission = learningGroup.AddPermission(
                LearningPermissions.BookReadingHistories.Default,
                L("Permission:BookReadingHistories"));
            bookHistoriesPermission.AddChild(LearningPermissions.BookReadingHistories.Create, L("Permission:BookReadingHistories.Create"));
            bookHistoriesPermission.AddChild(LearningPermissions.BookReadingHistories.Update, L("Permission:BookReadingHistories.Update"));
            bookHistoriesPermission.AddChild(LearningPermissions.BookReadingHistories.Delete, L("Permission:BookReadingHistories.Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<LearningResource>(name);
        }
    }
}
