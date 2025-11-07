using System;
using NouAssist.CollaborationHub.TeamCollaboration.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CollaborationHub.TeamCollaboration.Events
{
    [EventName("NouAssist.CollaborationHub.TeamCollaboration.MemberAdded")]
    public class TeamMemberAddedEto : IMultiTenant
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
        public TeamMemberRole Role { get; set; }
        public Guid? InvitedBy { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime AddedAt { get; set; }
    }

    [EventName("NouAssist.CollaborationHub.TeamCollaboration.MemberRemoved")]
    public class TeamMemberRemovedEto : IMultiTenant
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
        public string Reason { get; set; }
        public Guid? RemovedBy { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime RemovedAt { get; set; }
    }

    [EventName("NouAssist.CollaborationHub.TeamCollaboration.MemberRoleChanged")]
    public class TeamMemberRoleChangedEto : IMultiTenant
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
        public TeamMemberRole OldRole { get; set; }
        public TeamMemberRole NewRole { get; set; }
        public Guid? ChangedBy { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
