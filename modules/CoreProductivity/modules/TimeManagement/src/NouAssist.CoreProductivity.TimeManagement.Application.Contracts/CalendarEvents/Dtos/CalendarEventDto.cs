using System;
using System.Collections.Generic;
using NouAssist.CoreProductivity.TimeManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CoreProductivity.TimeManagement.CalendarEvents.Dtos
{
    public class CalendarEventDto : FullAuditedEntityDto<Guid>
    {
        public Guid CalendarId { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string EventLocation { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Timezone { get; set; }
        public bool IsAllDay { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string CancelledBy { get; set; }
        public string CancellationReason { get; set; }
        public bool IsRecurring { get; set; }
        public RecurrenceRuleDto RecurrenceRule { get; set; }
        public bool AllowOverlap { get; set; }
        public int DurationInMinutes { get; set; }
        public EventStatus Status { get; set; }
        public List<LinkedEntityDto> LinkedEntities { get; set; }
    }

    public class CreateCalendarEventDto
    {
        public Guid CalendarId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EventLocation { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Timezone { get; set; }
        public bool IsAllDay { get; set; }
        public bool IsRecurring { get; set; }
        public RecurrenceRuleDto RecurrenceRule { get; set; }
        public bool AllowOverlap { get; set; }
    }

    public class UpdateCalendarEventDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string EventLocation { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool IsAllDay { get; set; }
    }

    public class RescheduleEventDto
    {
        public DateTime NewStartAt { get; set; }
        public DateTime NewEndAt { get; set; }
    }

    public class CancelEventDto
    {
        public string CancellationReason { get; set; }
    }

    public class GetCalendarEventListDto : PagedAndSortedResultRequestDto
    {
        public Guid? CalendarId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsRecurring { get; set; }
        public bool IncludeCancelled { get; set; }
    }

    public class RecurrenceRuleDto
    {
        public string Frequency { get; set; }
        public int Interval { get; set; }
        public DateTime? Until { get; set; }
        public int? Count { get; set; }
        public List<int> ByDay { get; set; }
        public List<int> ByMonthDay { get; set; }
        public List<int> ByMonth { get; set; }
    }

    public class LinkedEntityDto
    {
        public Guid EntityId { get; set; }
        public LinkedEntityType EntityType { get; set; }
    }

    public class LinkEntityToEventDto
    {
        public Guid EntityId { get; set; }
        public LinkedEntityType EntityType { get; set; }
    }
}
