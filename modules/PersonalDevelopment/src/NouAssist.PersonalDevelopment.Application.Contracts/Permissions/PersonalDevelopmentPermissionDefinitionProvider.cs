using NouAssist.PersonalDevelopment.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.PersonalDevelopment.Permissions;

public class PersonalDevelopmentPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(PersonalDevelopmentPermissions.GroupName, L("Permission:PersonalDevelopment"));

        // Goals Permissions
        var goalsGroup = context.AddGroup(
            PersonalDevelopmentPermissions.Goal.GroupName,
            L("Permission:Goals"));

        var goalsPermission = goalsGroup.AddPermission(
            PersonalDevelopmentPermissions.Goal.Goals.Default, L("Permission:Goals"));
        goalsPermission.AddChild(PersonalDevelopmentPermissions.Goal.Goals.Create, L("Permission:Goals.Create"));
        goalsPermission.AddChild(PersonalDevelopmentPermissions.Goal.Goals.Update, L("Permission:Goals.Update"));
        goalsPermission.AddChild(PersonalDevelopmentPermissions.Goal.Goals.Delete, L("Permission:Goals.Delete"));
        goalsPermission.AddChild(PersonalDevelopmentPermissions.Goal.Goals.UpdateProgress, L("Permission:Goals.UpdateProgress"));
        goalsPermission.AddChild(PersonalDevelopmentPermissions.Goal.Goals.Achieve, L("Permission:Goals.Achieve"));
        goalsPermission.AddChild(PersonalDevelopmentPermissions.Goal.Goals.Archive, L("Permission:Goals.Archive"));

        var achievementsPermission = goalsGroup.AddPermission(
            PersonalDevelopmentPermissions.Goal.Achievements.Default, L("Permission:Achievements"));
        achievementsPermission.AddChild(PersonalDevelopmentPermissions.Goal.Achievements.View, L("Permission:Achievements.View"));
        achievementsPermission.AddChild(PersonalDevelopmentPermissions.Goal.Achievements.Manage, L("Permission:Achievements.Manage"));

        // Habits Permissions
        var habitsGroup = context.AddGroup(
            PersonalDevelopmentPermissions.Habit.GroupName,
            L("Permission:Habits"));

        var habitsPermission = habitsGroup.AddPermission(
            PersonalDevelopmentPermissions.Habit.Habits.Default, L("Permission:Habits"));
        habitsPermission.AddChild(PersonalDevelopmentPermissions.Habit.Habits.Create, L("Permission:Habits.Create"));
        habitsPermission.AddChild(PersonalDevelopmentPermissions.Habit.Habits.Edit, L("Permission:Habits.Edit"));
        habitsPermission.AddChild(PersonalDevelopmentPermissions.Habit.Habits.Delete, L("Permission:Habits.Delete"));
        habitsPermission.AddChild(PersonalDevelopmentPermissions.Habit.Habits.Track, L("Permission:Habits.Track"));

        var routinesPermission = habitsGroup.AddPermission(
            PersonalDevelopmentPermissions.Habit.Routines.Default, L("Permission:Routines"));
        routinesPermission.AddChild(PersonalDevelopmentPermissions.Habit.Routines.Create, L("Permission:Routines.Create"));
        routinesPermission.AddChild(PersonalDevelopmentPermissions.Habit.Routines.Edit, L("Permission:Routines.Edit"));
        routinesPermission.AddChild(PersonalDevelopmentPermissions.Habit.Routines.Delete, L("Permission:Routines.Delete"));
        routinesPermission.AddChild(PersonalDevelopmentPermissions.Habit.Routines.Complete, L("Permission:Routines.Complete"));

        var trackingPermission = habitsGroup.AddPermission(
            PersonalDevelopmentPermissions.Habit.Tracking.Default, L("Permission:Tracking"));
        trackingPermission.AddChild(PersonalDevelopmentPermissions.Habit.Tracking.ViewOwn, L("Permission:Tracking.ViewOwn"));
        trackingPermission.AddChild(PersonalDevelopmentPermissions.Habit.Tracking.ManageOwn, L("Permission:Tracking.ManageOwn"));

        var analyticsPermission = habitsGroup.AddPermission(
            PersonalDevelopmentPermissions.Habit.Analytics.Default, L("Permission:Analytics"));
        analyticsPermission.AddChild(PersonalDevelopmentPermissions.Habit.Analytics.ViewOwn, L("Permission:Analytics.ViewOwn"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PersonalDevelopmentResource>(name);
    }
}
