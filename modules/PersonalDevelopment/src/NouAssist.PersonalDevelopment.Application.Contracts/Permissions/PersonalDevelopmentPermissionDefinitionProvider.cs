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
            PersonalDevelopmentPermissions.Goals.GroupName,
            L("Permission:Goals"));

        var goalsPermission = goalsGroup.AddPermission(
            PersonalDevelopmentPermissions.Goals.Goals.Default, L("Permission:Goals"));
        goalsPermission.AddChild(PersonalDevelopmentPermissions.Goals.Goals.Create, L("Permission:Goals.Create"));
        goalsPermission.AddChild(PersonalDevelopmentPermissions.Goals.Goals.Update, L("Permission:Goals.Update"));
        goalsPermission.AddChild(PersonalDevelopmentPermissions.Goals.Goals.Delete, L("Permission:Goals.Delete"));
        goalsPermission.AddChild(PersonalDevelopmentPermissions.Goals.Goals.UpdateProgress, L("Permission:Goals.UpdateProgress"));
        goalsPermission.AddChild(PersonalDevelopmentPermissions.Goals.Goals.Achieve, L("Permission:Goals.Achieve"));
        goalsPermission.AddChild(PersonalDevelopmentPermissions.Goals.Goals.Archive, L("Permission:Goals.Archive"));

        var achievementsPermission = goalsGroup.AddPermission(
            PersonalDevelopmentPermissions.Goals.Achievements.Default, L("Permission:Achievements"));
        achievementsPermission.AddChild(PersonalDevelopmentPermissions.Goals.Achievements.View, L("Permission:Achievements.View"));
        achievementsPermission.AddChild(PersonalDevelopmentPermissions.Goals.Achievements.Manage, L("Permission:Achievements.Manage"));

        // Habits Permissions
        var habitsGroup = context.AddGroup(
            PersonalDevelopmentPermissions.Habits.GroupName,
            L("Permission:Habits"));

        var habitsPermission = habitsGroup.AddPermission(
            PersonalDevelopmentPermissions.Habits.Habits.Default, L("Permission:Habits"));
        habitsPermission.AddChild(PersonalDevelopmentPermissions.Habits.Habits.Create, L("Permission:Habits.Create"));
        habitsPermission.AddChild(PersonalDevelopmentPermissions.Habits.Habits.Edit, L("Permission:Habits.Edit"));
        habitsPermission.AddChild(PersonalDevelopmentPermissions.Habits.Habits.Delete, L("Permission:Habits.Delete"));
        habitsPermission.AddChild(PersonalDevelopmentPermissions.Habits.Habits.Track, L("Permission:Habits.Track"));

        var routinesPermission = habitsGroup.AddPermission(
            PersonalDevelopmentPermissions.Habits.Routines.Default, L("Permission:Routines"));
        routinesPermission.AddChild(PersonalDevelopmentPermissions.Habits.Routines.Create, L("Permission:Routines.Create"));
        routinesPermission.AddChild(PersonalDevelopmentPermissions.Habits.Routines.Edit, L("Permission:Routines.Edit"));
        routinesPermission.AddChild(PersonalDevelopmentPermissions.Habits.Routines.Delete, L("Permission:Routines.Delete"));
        routinesPermission.AddChild(PersonalDevelopmentPermissions.Habits.Routines.Complete, L("Permission:Routines.Complete"));

        var trackingPermission = habitsGroup.AddPermission(
            PersonalDevelopmentPermissions.Habits.Tracking.Default, L("Permission:Tracking"));
        trackingPermission.AddChild(PersonalDevelopmentPermissions.Habits.Tracking.ViewOwn, L("Permission:Tracking.ViewOwn"));
        trackingPermission.AddChild(PersonalDevelopmentPermissions.Habits.Tracking.ManageOwn, L("Permission:Tracking.ManageOwn"));

        var analyticsPermission = habitsGroup.AddPermission(
            PersonalDevelopmentPermissions.Habits.Analytics.Default, L("Permission:Analytics"));
        analyticsPermission.AddChild(PersonalDevelopmentPermissions.Habits.Analytics.ViewOwn, L("Permission:Analytics.ViewOwn"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PersonalDevelopmentResource>(name);
    }
}
