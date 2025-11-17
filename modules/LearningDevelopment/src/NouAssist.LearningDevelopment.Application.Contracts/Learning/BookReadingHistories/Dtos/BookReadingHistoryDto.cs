using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.LearningDevelopment.Learning.BookReadingHistories.Dtos
{
    public class BookReadingHistoryDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public Guid? TopicId { get; set; }
        public string BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CoverImageUrl { get; set; }
        public int LastReadPage { get; set; }
        public int? TotalPages { get; set; }
        public double? ProgressPercentage { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class CreateBookReadingHistoryDto
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CoverImageUrl { get; set; }
        public Guid? TopicId { get; set; }
    }

    public class UpdateBookProgressDto
    {
        public int LastReadPage { get; set; }
        public int? TotalPages { get; set; }
    }
}
