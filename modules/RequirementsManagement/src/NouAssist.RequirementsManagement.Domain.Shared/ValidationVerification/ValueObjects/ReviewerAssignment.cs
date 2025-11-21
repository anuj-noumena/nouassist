using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.ValidationVerification.ValueObjects
{
    /// <summary>
    /// Represents the assignment of a reviewer to a review task
    /// </summary>
    public class ReviewerAssignment : ValueObject
    {
        public Guid ReviewerId { get; private set; }
        public string ReviewerName { get; private set; }
        public string ReviewerEmail { get; private set; }
        public DateTime AssignedAt { get; private set; }
        public DateTime? DueDate { get; private set; }
        public bool IsRequired { get; private set; }

        private ReviewerAssignment() { }

        public ReviewerAssignment(
            Guid reviewerId,
            string reviewerName,
            string reviewerEmail,
            DateTime assignedAt,
            DateTime? dueDate = null,
            bool isRequired = true)
        {
            ReviewerId = reviewerId;
            ReviewerName = Check.NotNullOrWhiteSpace(reviewerName, nameof(reviewerName));
            ReviewerEmail = Check.NotNullOrWhiteSpace(reviewerEmail, nameof(reviewerEmail));
            AssignedAt = assignedAt;
            DueDate = dueDate;
            IsRequired = isRequired;
        }

        public bool IsOverdue(DateTime currentTime)
        {
            return DueDate.HasValue && currentTime > DueDate.Value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ReviewerId;
            yield return ReviewerName;
            yield return ReviewerEmail;
            yield return AssignedAt;
            yield return DueDate;
            yield return IsRequired;
        }
    }
}
