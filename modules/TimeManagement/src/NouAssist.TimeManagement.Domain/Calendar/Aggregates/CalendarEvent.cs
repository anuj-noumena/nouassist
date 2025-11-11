using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.TimeManagement.Calendar.Enums;
using NouAssist.TimeManagement.Calendar.Events;
using NouAssist.TimeManagement.Calendar.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.TimeManagement.Calendar.Aggregates
{
    public class CalendarEvent : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid CalendarId { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string EventLocation { get; set; }
        public DateTime StartAt { get; private set; }
        public DateTime EndAt { get; private set; }
        public string Timezone { get; set; }
        public bool IsAllDay { get; private set; }
        public bool IsCancelled { get; private set; }
        public DateTime? CancelledAt { get; private set; }
        public string CancelledBy { get; private set; }
        public string CancellationReason { get; private set; }
        public bool IsRecurring { get; private set; }
        public RecurrenceRule RecurrenceRule { get; private set; }
        public bool AllowOverlap { get; set; }

        private readonly List<LinkedEntity> _linkedEntities = new();
        public IReadOnlyCollection<LinkedEntity> LinkedEntities => _linkedEntities.AsReadOnly();

        private readonly List<Entities.CalendarEventException> _exceptions = new();
        public IReadOnlyCollection<Entities.CalendarEventException> Exceptions => _exceptions.AsReadOnly();

        protected CalendarEvent() { }

        public CalendarEvent(
            Guid id,
            Guid calendarId,
            Guid ownerId,
            string title,
            DateTime startAt,
            DateTime endAt,
            Guid? tenantId = null,
            string eventLocation = null,
            string description = null,
            bool isAllDay = false,
            bool isRecurring = false,
            RecurrenceRule recurrenceRule = null,
            string timezone = "UTC")
            : base(id)
        {
            CalendarId = calendarId;
            OwnerId = ownerId;
            SetTitle(title);
            SetTimeRange(startAt, endAt, isAllDay);
            TenantId = tenantId;
            EventLocation = eventLocation;
            Description = description;
            IsAllDay = isAllDay;
            IsRecurring = isRecurring;
            RecurrenceRule = recurrenceRule;
            Timezone = timezone ?? "UTC";
            IsCancelled = false;

            AddDistributedEvent(new CalendarEventCreatedEto
            {
                EventId = Id,
                CalendarId = CalendarId,
                OwnerId = OwnerId,
                TenantId = TenantId,
                Title = Title,
                StartAt = StartAt,
                EndAt = EndAt,
                IsRecurring = IsRecurring,
                CreatedAt = DateTime.UtcNow
            });
        }

        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 200);
        }

        public void SetTimeRange(DateTime startAt, DateTime endAt, bool isAllDay)
        {
            if (startAt >= endAt)
                throw new BusinessException("Calendar:InvalidTimeRange");

            var duration = endAt - startAt;

            if (isAllDay && duration.TotalDays < 1)
                throw new BusinessException("Calendar:AllDayEventMinDuration");

            if (!isAllDay && duration.TotalMinutes < 1)
                throw new BusinessException("Calendar:InvalidEventDuration");

            StartAt = startAt;
            EndAt = endAt;
            IsAllDay = isAllDay;
        }

        public void Reschedule(DateTime newStartAt, DateTime newEndAt)
        {
            if (IsCancelled)
                throw new BusinessException("Calendar:CannotEditCancelledEvent");

            SetTimeRange(newStartAt, newEndAt, IsAllDay);
        }

        public void Cancel(string cancelledBy, string cancellationReason = null)
        {
            if (IsCancelled)
                throw new BusinessException("Calendar:EventAlreadyCancelled");

            IsCancelled = true;
            CancelledAt = DateTime.UtcNow;
            CancelledBy = Check.NotNullOrWhiteSpace(cancelledBy, nameof(cancelledBy));
            CancellationReason = cancellationReason;

            AddDistributedEvent(new CalendarEventCancelledEto
            {
                EventId = Id,
                TenantId = TenantId,
                CancelledAt = CancelledAt.Value,
                CancelledBy = CancelledBy,
                CancellationReason = CancellationReason
            });
        }

        public void LinkToEntity(Guid entityId, LinkedEntityType entityType)
        {
            var entity = new LinkedEntity(entityId, entityType);
            
            if (_linkedEntities.Any(e => e.EntityId == entityId && e.EntityType == entityType))
                throw new BusinessException("Calendar:DuplicateLinkedEntity");

            _linkedEntities.Add(entity);
        }

        public void UnlinkEntity(Guid entityId, LinkedEntityType entityType)
        {
            _linkedEntities.RemoveAll(e => e.EntityId == entityId && e.EntityType == entityType);
        }

        public void AddException(Entities.CalendarEventException exception)
        {
            Check.NotNull(exception, nameof(exception));
            _exceptions.Add(exception);
        }

        public TimeSpan Duration => EndAt - StartAt;
        public int DurationInMinutes => (int)Duration.TotalMinutes;
        
        public EventStatus Status
        {
            get
            {
                if (IsCancelled) return EventStatus.Cancelled;
                var now = DateTime.UtcNow;
                if (now < StartAt) return EventStatus.Upcoming;
                if (now > EndAt) return EventStatus.Past;
                if (now >= StartAt && now <= EndAt) return EventStatus.Ongoing;
                return EventStatus.Scheduled;
            }
        }
    }
}
