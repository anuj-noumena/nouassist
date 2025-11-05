using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.Meetings.ValueObjects
{
    public class MeetingAgendaItem : ValueObject
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid? PresenterId { get; private set; }
        public int? DurationMinutes { get; private set; }
        public int OrderIndex { get; private set; }
        public bool IsCompleted { get; private set; }
        public string Notes { get; private set; }

        private MeetingAgendaItem() { /* ORM */ }

        public MeetingAgendaItem(
            string title,
            string description = null,
            int? durationMinutes = null,
            int orderIndex = 0
        )
        {
            Id = Guid.NewGuid();
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = description;
            DurationMinutes = durationMinutes;
            OrderIndex = orderIndex;
            IsCompleted = false;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return PresenterId;
            yield return DurationMinutes;
            yield return OrderIndex;
        }
    }
}
