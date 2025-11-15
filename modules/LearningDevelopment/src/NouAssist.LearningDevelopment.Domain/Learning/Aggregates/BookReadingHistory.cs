using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.Learning.Aggregates
{
    /// <summary>
    /// Aggregate root for tracking book reading progress
    /// </summary>
    public class BookReadingHistory : FullAuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; private set; }
        public Guid? TopicId { get; private set; }
        public string BookId { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string CoverImageUrl { get; private set; }
        public int LastReadPage { get; private set; }
        public int? TotalPages { get; private set; }
        public double? ProgressPercentage { get; private set; }
        public DateTime Timestamp { get; private set; }

        protected BookReadingHistory() { /* ORM */ }

        public BookReadingHistory(
            Guid id,
            Guid userId,
            string bookId,
            string title,
            Guid? topicId = null)
            : base(id)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            BookId = Check.NotNullOrWhiteSpace(bookId, nameof(bookId));
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            TopicId = topicId;
            Timestamp = Clock.Now;
            LastReadPage = 0;
        }

        public void UpdateProgress(int lastReadPage, int? totalPages = null)
        {
            LastReadPage = lastReadPage >= 0
                ? lastReadPage
                : throw new ArgumentException("Last read page cannot be negative");

            if (totalPages.HasValue)
            {
                TotalPages = totalPages.Value;
                ProgressPercentage = totalPages.Value > 0
                    ? (double)LastReadPage / totalPages.Value
                    : 0;
            }

            Timestamp = Clock.Now;
        }

        public void UpdateBookDetails(string title, string author, string coverImageUrl)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Author = author;
            CoverImageUrl = coverImageUrl;
        }

        public void AssociateWithTopic(Guid topicId)
        {
            TopicId = topicId;
        }

        public void DisassociateFromTopic()
        {
            TopicId = null;
        }
    }
}
