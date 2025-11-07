using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.ReflectionAndSupport.Reviews.ValueObjects
{
    public class CompletedChecklistItem : ValueObject
    {
        public Guid Id { get; init; }
        public string Description { get; init; }
        public bool IsCompleted { get; init; }
        public DateTime? CompletedAt { get; init; }
        public string CompletedBy { get; init; }

        public CompletedChecklistItem(Guid id, string description)
        {
            Id = id;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 500);
            IsCompleted = false;
        }

        private CompletedChecklistItem(
            Guid id, 
            string description, 
            bool isCompleted, 
            DateTime? completedAt, 
            string completedBy
        )
        {
            Id = id;
            Description = description;
            IsCompleted = isCompleted;
            CompletedAt = completedAt;
            CompletedBy = completedBy;
        }

        public CompletedChecklistItem Complete(string completedBy)
        {
            if (IsCompleted) return this;

            if (string.IsNullOrWhiteSpace(completedBy))
            {
                throw new BusinessException("Reviews:MissingCompletedBy", 
                    "Must specify who completed the item");
            }

            return new CompletedChecklistItem(
                Id,
                Description,
                true,
                DateTime.UtcNow,
                completedBy
            );
        }

        public CompletedChecklistItem Uncomplete()
        {
            return new CompletedChecklistItem(Id, Description, false, null, null);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Description;
            yield return IsCompleted;
            yield return CompletedAt;
            yield return CompletedBy ?? string.Empty;
        }
    }
}
