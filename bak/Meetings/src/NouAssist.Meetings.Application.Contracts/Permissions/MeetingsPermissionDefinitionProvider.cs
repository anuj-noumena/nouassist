using NouAssist.Meetings.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Meetings.Permissions
{
    public class MeetingsPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var meetingsGroup = context.AddGroup(
                MeetingsPermissions.GroupName,
                L("Permission:Meetings"));

            var meetingsPermission = meetingsGroup.AddPermission(
                MeetingsPermissions.Meetings.Default,
                L("Permission:Meetings"));
            meetingsPermission.AddChild(
                MeetingsPermissions.Meetings.Create,
                L("Permission:Meetings.Create"));
            meetingsPermission.AddChild(
                MeetingsPermissions.Meetings.Update,
                L("Permission:Meetings.Update"));
            meetingsPermission.AddChild(
                MeetingsPermissions.Meetings.Delete,
                L("Permission:Meetings.Delete"));
            meetingsPermission.AddChild(
                MeetingsPermissions.Meetings.Cancel,
                L("Permission:Meetings.Cancel"));
            meetingsPermission.AddChild(
                MeetingsPermissions.Meetings.Start,
                L("Permission:Meetings.Start"));
            meetingsPermission.AddChild(
                MeetingsPermissions.Meetings.Complete,
                L("Permission:Meetings.Complete"));
            meetingsPermission.AddChild(
                MeetingsPermissions.Meetings.ManageAttendees,
                L("Permission:Meetings.ManageAttendees"));
            meetingsPermission.AddChild(
                MeetingsPermissions.Meetings.ManageAgenda,
                L("Permission:Meetings.ManageAgenda"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MeetingsResource>(name);
        }
    }
}
