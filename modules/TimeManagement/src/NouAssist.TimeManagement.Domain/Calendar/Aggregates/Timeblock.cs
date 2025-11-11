using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.TimeManagement.Calendar.Enums;
using NouAssist.TimeManagement.Calendar.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.TimeManagement.Calendar.Aggregates
{
    public class Timeblock : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid CalendarEventId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CompletionNotes { get; private set; }
        public string Color { get; set; }
        public TimeblockType BlockType { get; set; }
        public TimeblockStatus Status { get; private set; }
        public DateTime StartAt { get; private set; }
        public DateTime EndAt { get; private set; }
        public string Timezone { get; set; }
        public bool IsFlexible { get; set; }
        public bool AllowOverlap { get; set; }
        public Guid? CalendarId { get; set; }

        private readonly List<Entities.TimeblockTask> _tasks = new();
        public IReadOnlyCollection<Entities.TimeblockTask> Tasks => _tasks.AsReadOnly();

        protected Timeblock() { }

        public Timeblock(
            Guid id,
            Guid ownerId,
            Guid calendarEventId,
            DateTime startAt,
            DateTime endAt,
            Guid? tenantId = null,
            TimeblockType blockType = TimeblockType.FocusWork,
            string title = null,
            string description = null,
            string timezone = "UTC")
            : base(id)
        {
            OwnerId = ownerId;
            CalendarEventId = calendarEventId;
            TenantId = tenantId;
            BlockType = blockType;
            Title = title;
            Description = description;
            Timezone = timezone ?? "UTC";
            Status = TimeblockStatus.Planned;
            SetTimeRange(startAt, endAt);
        }

        public void SetTimeRange(DateTime startAt, DateTime endAt)
        {
            if (startAt >= endAt)
                throw new BusinessException("Calendar:InvalidTimeRange");

            var duration = endAt - startAt;
            
            if (duration.TotalMinutes < 15)
                throw new BusinessException("Calendar:TimeblockMinDuration");

            if (duration.TotalHours > 8)
                throw new BusinessException("Calendar:TimeblockMaxDuration");

            StartAt = startAt;
            EndAt = endAt;
        }

        public void Start()
        {
            if (Status != TimeblockStatus.Planned)
                throw new BusinessException("Calendar:CanOnlyStartPlanned");

            Status = TimeblockStatus.InProgress;
        }

        public void Complete(string completionNotes = null)
        {
            if (Status == TimeblockStatus.Completed)
                throw new BusinessException("Calendar:TimeblockAlreadyCompleted");

            Status = TimeblockStatus.Completed;
            CompletionNotes = completionNotes;

            AddDistributedEvent(new TimeblockCompletedEto
            {
                TimeblockId = Id,
                OwnerId = OwnerId,
                TenantId = TenantId,
                CompletedAt = DateTime.UtcNow,
                DurationMinutes = DurationInMinutes,
                CompletionNotes = CompletionNotes
            });
        }

        public void Cancel()
        {
            Status = TimeblockStatus.Cancelled;
        }

        public void AddTask(Guid taskInstanceId, int estimatedMinutes = 0)
        {
            var task = new Entities.TimeblockTask(
                Guid.NewGuid(),
                Id,
                taskInstanceId,
                estimatedMinutes);

            _tasks.Add(task);
        }

        public TimeSpan Duration => EndAt - StartAt;
        public int DurationInMinutes => (int)Duration.TotalMinutes;
        public int TotalEstimatedMinutes => _tasks.Sum(t => t.EstimatedMinutes);
        public int TotalActualMinutes => _tasks.Sum(t => t.ActualMinutes);
    }
}
