using System;
using System.Collections.Generic;
using NouAssist.TimeManagement.Calendar.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.TimeManagement.Calendar.Timeblocks.Dtos
{
    public class TimeblockDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public Guid CalendarEventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CompletionNotes { get; set; }
        public string Color { get; set; }
        public TimeblockType BlockType { get; set; }
        public TimeblockStatus Status { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Timezone { get; set; }
        public bool IsFlexible { get; set; }
        public bool AllowOverlap { get; set; }
        public Guid? CalendarId { get; set; }
        public int DurationInMinutes { get; set; }
        public List<TimeblockTaskDto> Tasks { get; set; }
    }

    public class TimeblockTaskDto
    {
        public Guid Id { get; set; }
        public Guid TaskInstanceId { get; set; }
        public int EstimatedMinutes { get; set; }
        public int ActualMinutes { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class CreateTimeblockDto
    {
        public Guid CalendarEventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeblockType BlockType { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Timezone { get; set; }
        public bool IsFlexible { get; set; }
    }

    public class UpdateTimeblockDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }

    public class CompleteTimeblockDto
    {
        public string CompletionNotes { get; set; }
    }

    public class GetTimeblockListDto : PagedAndSortedResultRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeblockStatus? Status { get; set; }
        public TimeblockType? BlockType { get; set; }
    }

    public class AddTaskToTimeblockDto
    {
        public Guid TaskInstanceId { get; set; }
        public int EstimatedMinutes { get; set; }
    }
}
