using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.ValidationVerification.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.ValidationVerification.ValueObjects
{
    /// <summary>
    /// Represents a feedback comment with metadata
    /// </summary>
    public class FeedbackComment : ValueObject
    {
        public string Comment { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid AuthorId { get; private set; }
        public string AuthorName { get; private set; }
        public bool IsInternal { get; private set; }

        private FeedbackComment() { }

        public FeedbackComment(
            string comment,
            DateTime createdAt,
            Guid authorId,
            string authorName,
            bool isInternal = false)
        {
            SetComment(comment);
            CreatedAt = createdAt;
            AuthorId = authorId;
            AuthorName = Check.NotNullOrWhiteSpace(authorName, nameof(authorName));
            IsInternal = isInternal;
        }

        private void SetComment(string comment)
        {
            Comment = Check.NotNullOrWhiteSpace(
                comment,
                nameof(comment),
                maxLength: ValidationVerificationConsts.ReviewFeedback.MaxCommentLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Comment;
            yield return CreatedAt;
            yield return AuthorId;
            yield return AuthorName;
            yield return IsInternal;
        }
    }
}
