using NouAssist.CoreProductivity.TimeManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CoreProductivity.TimeManagement.Permissions
{
    public class CalendarPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var calendarGroup = context.AddGroup(CalendarPermissions.GroupName, L("Permission:Calendar"));

            // Calendars
            var calendarsPermission = calendarGroup.AddPermission(
                CalendarPermissions.Calendars.Default, L("Permission:Calendars"));
            calendarsPermission.AddChild(CalendarPermissions.Calendars.Create, L("Permission:Calendars.Create"));
            calendarsPermission.AddChild(CalendarPermissions.Calendars.Edit, L("Permission:Calendars.Edit"));
            calendarsPermission.AddChild(CalendarPermissions.Calendars.Delete, L("Permission:Calendars.Delete"));
            calendarsPermission.AddChild(CalendarPermissions.Calendars.Share, L("Permission:Calendars.Share"));

            // Calendar Events
            var eventsPermission = calendarGroup.AddPermission(
                CalendarPermissions.CalendarEvents.Default, L("Permission:CalendarEvents"));
            eventsPermission.AddChild(CalendarPermissions.CalendarEvents.Create, L("Permission:CalendarEvents.Create"));
            eventsPermission.AddChild(CalendarPermissions.CalendarEvents.Edit, L("Permission:CalendarEvents.Edit"));
            eventsPermission.AddChild(CalendarPermissions.CalendarEvents.Delete, L("Permission:CalendarEvents.Delete"));
            eventsPermission.AddChild(CalendarPermissions.CalendarEvents.Cancel, L("Permission:CalendarEvents.Cancel"));

            // Timeblocks
            var timeblocksPermission = calendarGroup.AddPermission(
                CalendarPermissions.Timeblocks.Default, L("Permission:Timeblocks"));
            timeblocksPermission.AddChild(CalendarPermissions.Timeblocks.Create, L("Permission:Timeblocks.Create"));
            timeblocksPermission.AddChild(CalendarPermissions.Timeblocks.Edit, L("Permission:Timeblocks.Edit"));
            timeblocksPermission.AddChild(CalendarPermissions.Timeblocks.Delete, L("Permission:Timeblocks.Delete"));
            timeblocksPermission.AddChild(CalendarPermissions.Timeblocks.Complete, L("Permission:Timeblocks.Complete"));

            // Scheduling Requests
            var schedulingPermission = calendarGroup.AddPermission(
                CalendarPermissions.SchedulingRequests.Default, L("Permission:SchedulingRequests"));
            schedulingPermission.AddChild(CalendarPermissions.SchedulingRequests.Create, L("Permission:SchedulingRequests.Create"));
            schedulingPermission.AddChild(CalendarPermissions.SchedulingRequests.Schedule, L("Permission:SchedulingRequests.Schedule"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<TimeManagementResource>(name);
        }
    }
}
