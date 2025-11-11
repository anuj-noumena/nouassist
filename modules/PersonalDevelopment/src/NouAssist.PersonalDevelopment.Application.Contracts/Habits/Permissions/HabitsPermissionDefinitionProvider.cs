using NouAssist.PersonalDevelopment.Habits.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.PersonalDevelopment.Habits.Permissions
{
    public class HabitsPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var habitsGroup = context.AddGroup(HabitsPermissions.GroupName, L("Permission:Habits"));

            var habitsPermission = habitsGroup.AddPermission(
                HabitsPermissions.Habits.Default, L("Permission:Habits"));
            habitsPermission.AddChild(HabitsPermissions.Habits.Create, L("Permission:Habits.Create"));
            habitsPermission.AddChild(HabitsPermissions.Habits.Edit, L("Permission:Habits.Edit"));
            habitsPermission.AddChild(HabitsPermissions.Habits.Delete, L("Permission:Habits.Delete"));
            habitsPermission.AddChild(HabitsPermissions.Habits.Track, L("Permission:Habits.Track"));

            var routinesPermission = habitsGroup.AddPermission(
                HabitsPermissions.Routines.Default, L("Permission:Routines"));
            routinesPermission.AddChild(HabitsPermissions.Routines.Create, L("Permission:Routines.Create"));
            routinesPermission.AddChild(HabitsPermissions.Routines.Edit, L("Permission:Routines.Edit"));
            routinesPermission.AddChild(HabitsPermissions.Routines.Delete, L("Permission:Routines.Delete"));
            routinesPermission.AddChild(HabitsPermissions.Routines.Complete, L("Permission:Routines.Complete"));

            var trackingPermission = habitsGroup.AddPermission(
                HabitsPermissions.Tracking.Default, L("Permission:Tracking"));
            trackingPermission.AddChild(HabitsPermissions.Tracking.ViewOwn, L("Permission:Tracking.ViewOwn"));
            trackingPermission.AddChild(HabitsPermissions.Tracking.ManageOwn, L("Permission:Tracking.ManageOwn"));

            var analyticsPermission = habitsGroup.AddPermission(
                HabitsPermissions.Analytics.Default, L("Permission:Analytics"));
            analyticsPermission.AddChild(HabitsPermissions.Analytics.ViewOwn, L("Permission:Analytics.ViewOwn"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<HabitsResource>(name);
        }
    }
}
