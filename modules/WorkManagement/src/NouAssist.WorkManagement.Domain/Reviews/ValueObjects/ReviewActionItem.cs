using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.Reviews.ValueObjects
{
    public class ReviewActionItem : ValueObject
    {
        public Guid Id { get; init; }
        public string Description { get; init; }
        public string AssignedTo { get; init; }
        public DateTime? DueBy { get; init; }
        public bool IsCompleted { get; init; }
        public DateTime? CompletedAt { get; init; }

        public ReviewActionItem(Guid id, string description, string assignedTo = null, DateTime? dueBy = null)
        {
            Id = id;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 500);
            AssignedTo = assignedTo;
            DueBy = dueBy;
            IsCompleted = false;
        }

        private ReviewActionItem(Guid id, string description, string assignedTo, DateTime? dueBy, bool isCompleted, DateTime? completedAt)
        {
            Id = id;
            Description = description;
            AssignedTo = assignedTo;
            DueBy = dueBy;
            IsCompleted = isCompleted;
            CompletedAt = completedAt;
        }

        public bool IsAssigned => !string.IsNullOrWhiteSpace(AssignedTo);
        public bool HasDueDate => DueBy.HasValue;
        public bool IsOverdue => !IsCompleted && DueBy.HasValue && DateTime.UtcNow > DueBy.Value;

        public ReviewActionItem Complete()
        {
            if (IsCompleted)
            {
                return this;
            }

            return new ReviewActionItem(Id, Description, AssignedTo, DueBy, true, DateTime.UtcNow);
        }

        public ReviewActionItem Uncomplete()
        {
            return new ReviewActionItem(Id, Description, AssignedTo, DueBy, false, null);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Description;
            yield return AssignedTo ?? string.Empty;
            yield return DueBy;
            yield return IsCompleted;
            yield return CompletedAt;
        }
    }
}
