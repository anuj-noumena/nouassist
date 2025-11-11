using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.TimeManagement.Meetings.ValueObjects
{
    public class MeetingActionItem : ValueObject
    {
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public Guid? AssignedToUserId { get; private set; }
        public DateTime? DueDate { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public string Notes { get; private set; }

        private MeetingActionItem() { /* ORM */ }

        public MeetingActionItem(
            string description,
            Guid? assignedToUserId = null,
            DateTime? dueDate = null
        )
        {
            Id = Guid.NewGuid();
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            AssignedToUserId = assignedToUserId;
            DueDate = dueDate;
            IsCompleted = false;
        }

        public bool IsAssigned => AssignedToUserId.HasValue;
        
        public bool IsOverdue =>
            !IsCompleted && DueDate.HasValue && DateTime.UtcNow > DueDate.Value;

        public MeetingActionItem Complete()
        {
            return new MeetingActionItem(Description, AssignedToUserId, DueDate)
            {
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow,
                Notes = this.Notes
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Description;
            yield return AssignedToUserId;
            yield return DueDate;
            yield return IsCompleted;
        }
    }
}
