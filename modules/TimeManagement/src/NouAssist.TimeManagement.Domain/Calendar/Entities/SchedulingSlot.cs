using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.TimeManagement.Calendar.Entities
{
    public class SchedulingSlot : Entity<Guid>
    {
        public Guid RequestId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public string Timezone { get; private set; }
        public int VotesCount { get; private set; }
        public bool IsSelected { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private readonly List<Guid> _availableParticipants = new();
        public IReadOnlyCollection<Guid> AvailableParticipants => _availableParticipants.AsReadOnly();

        protected SchedulingSlot() { }

        public SchedulingSlot(
            Guid id,
            Guid requestId,
            DateTime startTime,
            DateTime endTime,
            List<Guid> availableParticipants,
            string timezone = null)
            : base(id)
        {
            if (startTime >= endTime)
            {
                throw new BusinessException("Calendar:InvalidTimeRange");
            }

            RequestId = requestId;
            StartTime = startTime;
            EndTime = endTime;
            Timezone = timezone;
            VotesCount = 0;
            IsSelected = false;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            if (availableParticipants != null && availableParticipants.Any())
            {
                _availableParticipants.AddRange(availableParticipants);
            }
        }

        public void AddVote()
        {
            VotesCount++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Select()
        {
            IsSelected = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public TimeSpan Duration => EndTime - StartTime;
        public int DurationMinutes => (int)Duration.TotalMinutes;
        public int AvailableCount => _availableParticipants.Count;
    }
}
