using NouAssist.TimeManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.TimeManagement.Permissions;

public class TimeManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var timeManagementGroup = context.AddGroup(TimeManagementPermissions.GroupName, L("Permission:TimeManagement"));

        // Calendar Permissions
        var calendarsPermission = timeManagementGroup.AddPermission(
            TimeManagementPermissions.Calendar.Calendars.Default, L("Permission:Calendars"));
        calendarsPermission.AddChild(TimeManagementPermissions.Calendar.Calendars.Create, L("Permission:Calendars.Create"));
        calendarsPermission.AddChild(TimeManagementPermissions.Calendar.Calendars.Edit, L("Permission:Calendars.Edit"));
        calendarsPermission.AddChild(TimeManagementPermissions.Calendar.Calendars.Delete, L("Permission:Calendars.Delete"));
        calendarsPermission.AddChild(TimeManagementPermissions.Calendar.Calendars.Share, L("Permission:Calendars.Share"));

        var eventsPermission = timeManagementGroup.AddPermission(
            TimeManagementPermissions.Calendar.CalendarEvents.Default, L("Permission:CalendarEvents"));
        eventsPermission.AddChild(TimeManagementPermissions.Calendar.CalendarEvents.Create, L("Permission:CalendarEvents.Create"));
        eventsPermission.AddChild(TimeManagementPermissions.Calendar.CalendarEvents.Edit, L("Permission:CalendarEvents.Edit"));
        eventsPermission.AddChild(TimeManagementPermissions.Calendar.CalendarEvents.Delete, L("Permission:CalendarEvents.Delete"));
        eventsPermission.AddChild(TimeManagementPermissions.Calendar.CalendarEvents.Cancel, L("Permission:CalendarEvents.Cancel"));

        var timeblocksPermission = timeManagementGroup.AddPermission(
            TimeManagementPermissions.Calendar.Timeblocks.Default, L("Permission:Timeblocks"));
        timeblocksPermission.AddChild(TimeManagementPermissions.Calendar.Timeblocks.Create, L("Permission:Timeblocks.Create"));
        timeblocksPermission.AddChild(TimeManagementPermissions.Calendar.Timeblocks.Edit, L("Permission:Timeblocks.Edit"));
        timeblocksPermission.AddChild(TimeManagementPermissions.Calendar.Timeblocks.Delete, L("Permission:Timeblocks.Delete"));
        timeblocksPermission.AddChild(TimeManagementPermissions.Calendar.Timeblocks.Complete, L("Permission:Timeblocks.Complete"));

        var schedulingPermission = timeManagementGroup.AddPermission(
            TimeManagementPermissions.Calendar.SchedulingRequests.Default, L("Permission:SchedulingRequests"));
        schedulingPermission.AddChild(TimeManagementPermissions.Calendar.SchedulingRequests.Create, L("Permission:SchedulingRequests.Create"));
        schedulingPermission.AddChild(TimeManagementPermissions.Calendar.SchedulingRequests.Schedule, L("Permission:SchedulingRequests.Schedule"));

        // Meetings Permissions
        var meetingsPermission = timeManagementGroup.AddPermission(
            TimeManagementPermissions.Meeting.Meetings.Default,
            L("Permission:Meetings"));
        meetingsPermission.AddChild(
            TimeManagementPermissions.Meeting.Meetings.Create,
            L("Permission:Meetings.Create"));
        meetingsPermission.AddChild(
            TimeManagementPermissions.Meeting.Meetings.Update,
            L("Permission:Meetings.Update"));
        meetingsPermission.AddChild(
            TimeManagementPermissions.Meeting.Meetings.Delete,
            L("Permission:Meetings.Delete"));
        meetingsPermission.AddChild(
            TimeManagementPermissions.Meeting.Meetings.Cancel,
            L("Permission:Meetings.Cancel"));
        meetingsPermission.AddChild(
            TimeManagementPermissions.Meeting.Meetings.Start,
            L("Permission:Meetings.Start"));
        meetingsPermission.AddChild(
            TimeManagementPermissions.Meeting.Meetings.Complete,
            L("Permission:Meetings.Complete"));
        meetingsPermission.AddChild(
            TimeManagementPermissions.Meeting.Meetings.ManageAttendees,
            L("Permission:Meetings.ManageAttendees"));
        meetingsPermission.AddChild(
            TimeManagementPermissions.Meeting.Meetings.ManageAgenda,
            L("Permission:Meetings.ManageAgenda"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<TimeManagementResource>(name);
    }
}
