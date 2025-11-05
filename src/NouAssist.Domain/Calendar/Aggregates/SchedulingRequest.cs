using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Calendar.Enums;
using NouAssist.Calendar.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace NouAssist.Calendar.Aggregates
{
    public class SchedulingRequest : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid RequesterId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public int DurationMinutes { get; private set; }
        public DateTime? PreferredDate { get; set; }
        public string PreferredTimeStart { get; set; }
        public string PreferredTimeEnd { get; set; }
        public SchedulingRequestStatus Status { get; private set; }
        public Guid? SelectedSlotId { get; private set; }
        public DateTime? ScheduledAt { get; private set; }
        public Guid? CalendarEventId { get; private set; }

        private readonly List<Guid> _participantIds = new();
        public IReadOnlyCollection<Guid> ParticipantIds => _participantIds.AsReadOnly();

        private readonly List<Entities.SchedulingSlot> _slots = new();
        public IReadOnlyCollection<Entities.SchedulingSlot> Slots => _slots.AsReadOnly();

        protected SchedulingRequest() { }

        public SchedulingRequest(
            Guid id,
            Guid requesterId,
            string title,
            int durationMinutes,
            List<Guid> participantIds,
            Guid? tenantId = null,
            string description = null)
            : base(id)
        {
            RequesterId = requesterId;
            SetTitle(title);
            SetDuration(durationMinutes);
            TenantId = tenantId;
            Description = description;
            Status = SchedulingRequestStatus.Pending;

            if (participantIds == null || !participantIds.Any())
                throw new BusinessException("Calendar:MinimumParticipants");

            _participantIds.AddRange(participantIds);
        }

        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 200);
        }

        public void SetDuration(int durationMinutes)
        {
            if (durationMinutes < 15 || durationMinutes > 480)
                throw new ArgumentException("Duration must be between 15 and 480 minutes", nameof(durationMinutes));

            DurationMinutes = durationMinutes;
        }

        public void AddSlot(DateTime startTime, DateTime endTime, List<Guid> availableParticipants, string timezone = null)
        {
            var slot = new Entities.SchedulingSlot(
                Guid.NewGuid(),
                Id,
                startTime,
                endTime,
                availableParticipants,
                timezone);

            _slots.Add(slot);
        }

        public void Schedule(Guid slotId, Guid calendarEventId)
        {
            if (Status != SchedulingRequestStatus.Pending)
                throw new BusinessException("Calendar:SchedulingRequestNotPending");

            var slot = _slots.FirstOrDefault(s => s.Id == slotId);
            if (slot == null)
                throw new EntityNotFoundException(typeof(Entities.SchedulingSlot), slotId);

            slot.Select();
            Status = SchedulingRequestStatus.Scheduled;
            SelectedSlotId = slotId;
            CalendarEventId = calendarEventId;
            ScheduledAt = DateTime.UtcNow;

            AddDistributedEvent(new SchedulingRequestAcceptedEto
            {
                RequestId = Id,
                RequesterId = RequesterId,
                TenantId = TenantId,
                SelectedSlotId = slotId,
                CalendarEventId = calendarEventId,
                ScheduledAt = ScheduledAt.Value
            });
        }

        public void Cancel()
        {
            Status = SchedulingRequestStatus.Declined;
        }

        public double DurationHours => DurationMinutes / 60.0;
    }
}
