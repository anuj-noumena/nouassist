using System;
using System.Collections.Generic;
using NouAssist.CollaborationHub.TeamCollaboration.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CollaborationHub.TeamCollaboration.Events
{
    [EventName("NouAssist.CollaborationHub.TeamCollaboration.MeetingScheduled")]
    public class MeetingScheduledEto : IMultiTenant
    {
        public Guid MeetingId { get; set; }
        public string Title { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public Guid OrganizerId { get; set; }
        public MeetingType MeetingType { get; set; }
        public List<Guid> AttendeeIds { get; set; }
        public Guid? TenantId { get; set; }
    }

    [EventName("NouAssist.CollaborationHub.TeamCollaboration.MeetingCancelled")]
    public class MeetingCancelledEto : IMultiTenant
    {
        public Guid MeetingId { get; set; }
        public string Title { get; set; }
        public string Reason { get; set; }
        public DateTime CancelledAt { get; set; }
        public Guid? TenantId { get; set; }
    }

    [EventName("NouAssist.CollaborationHub.TeamCollaboration.MeetingCompleted")]
    public class MeetingCompletedEto : IMultiTenant
    {
        public Guid MeetingId { get; set; }
        public string Title { get; set; }
        public DateTime CompletedAt { get; set; }
        public int AttendeeCount { get; set; }
        public int ActionItemCount { get; set; }
        public bool HasRecording { get; set; }
        public Guid? TenantId { get; set; }
    }
}
