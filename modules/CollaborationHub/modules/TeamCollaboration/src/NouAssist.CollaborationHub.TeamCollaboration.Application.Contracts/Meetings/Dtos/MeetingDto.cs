using System;
using System.Collections.Generic;
using NouAssist.CollaborationHub.TeamCollaboration.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CollaborationHub.TeamCollaboration.Meetings.Dtos
{
    public class MeetingDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Objective { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Location { get; set; }
        public string MeetingUrl { get; set; }
        public MeetingType MeetingType { get; set; }
        public MeetingFrequency Frequency { get; set; }
        public MeetingStatus Status { get; set; }
        public Guid? OrganizerId { get; set; }
        public string OrganizerName { get; set; }
        public int AttendeeCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int DeclinedCount { get; set; }
        public double AcceptanceRate { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsPast { get; set; }
        public bool IsHappeningNow { get; set; }
        public bool HasRecording { get; set; }
        public string RecordingUrl { get; set; }
        public bool IsCancelled { get; set; }
        public string CancellationReason { get; set; }
        public List<MeetingAttendeeDto> Attendees { get; set; }
        public List<MeetingAgendaItemDto> AgendaItems { get; set; }
        public List<MeetingActionItemDto> ActionItems { get; set; }
    }

    public class MeetingAttendeeDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public AttendeeRole Role { get; set; }
        public AttendeeStatus Status { get; set; }
        public bool IsRequired { get; set; }
        public bool IsOrganizer { get; set; }
    }

    public class MeetingAgendaItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? PresenterId { get; set; }
        public string PresenterName { get; set; }
        public int? DurationMinutes { get; set; }
        public int OrderIndex { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class MeetingActionItemDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public string AssignedToName { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOverdue { get; set; }
    }

    public class CreateMeetingDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Objective { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Location { get; set; }
        public string MeetingUrl { get; set; }
        public MeetingType MeetingType { get; set; }
        public MeetingFrequency Frequency { get; set; }
        public List<AddMeetingAttendeeDto> Attendees { get; set; }
        public List<CreateAgendaItemDto> AgendaItems { get; set; }
    }

    public class AddMeetingAttendeeDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public AttendeeRole Role { get; set; }
        public bool IsRequired { get; set; }
    }

    public class CreateAgendaItemDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? PresenterId { get; set; }
        public int? DurationMinutes { get; set; }
    }

    public class UpdateMeetingDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Objective { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Location { get; set; }
        public string MeetingUrl { get; set; }
    }

    public class CompleteMeetingDto
    {
        public string Notes { get; set; }
        public string Decisions { get; set; }
        public string Summary { get; set; }
    }

    public class UpdateAttendeeStatusDto
    {
        public AttendeeStatus Status { get; set; }
    }

    public class AddActionItemDto
    {
        public string Description { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class GetMeetingListDto : PagedAndSortedResultRequestDto
    {
        public MeetingStatus? Status { get; set; }
        public MeetingType? MeetingType { get; set; }
        public Guid? ProjectId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? OnlyUpcoming { get; set; }
        public string Filter { get; set; }
    }
}
