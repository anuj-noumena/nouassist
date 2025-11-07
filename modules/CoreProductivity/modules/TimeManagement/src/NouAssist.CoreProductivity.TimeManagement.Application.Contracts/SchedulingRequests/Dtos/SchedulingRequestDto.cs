using System;
using System.Collections.Generic;
using NouAssist.CoreProductivity.TimeManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CoreProductivity.TimeManagement.SchedulingRequests.Dtos
{
    public class SchedulingRequestDto : FullAuditedEntityDto<Guid>
    {
        public Guid RequesterId { get; set; }
        public List<Guid> ParticipantIds { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime? PreferredDate { get; set; }
        public string PreferredTimeStart { get; set; }
        public string PreferredTimeEnd { get; set; }
        public SchedulingRequestStatus Status { get; set; }
        public Guid? SelectedSlotId { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public Guid? CalendarEventId { get; set; }
        public List<SchedulingSlotDto> Slots { get; set; }
    }

    public class SchedulingSlotDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Timezone { get; set; }
        public List<Guid> AvailableParticipants { get; set; }
        public int VotesCount { get; set; }
        public bool IsSelected { get; set; }
        public int DurationMinutes { get; set; }
        public int AvailableCount { get; set; }
    }

    public class CreateSchedulingRequestDto
    {
        public List<Guid> ParticipantIds { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime? PreferredDate { get; set; }
    }

    public class AddSchedulingSlotDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Guid> AvailableParticipants { get; set; }
        public string Timezone { get; set; }
    }

    public class ScheduleRequestDto
    {
        public Guid SlotId { get; set; }
        public Guid CalendarEventId { get; set; }
    }
}
