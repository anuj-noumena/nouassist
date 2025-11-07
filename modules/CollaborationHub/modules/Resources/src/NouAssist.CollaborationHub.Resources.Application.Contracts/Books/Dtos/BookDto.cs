using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CollaborationHub.Resources.Books.Dtos
{
    public class BookDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public Guid ResourceId { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string DisplayAuthors { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Edition { get; set; }
        public string Language { get; set; }
        public int? PageCount { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public string PrimaryIsbn { get; set; }
        public Guid? CoverImageResourceId { get; set; }
        public GoogleBookDataDto GoogleBookData { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
        public string ReadingStatus { get; set; }
        public int? CurrentPage { get; set; }
        public double? ProgressPercentage { get; set; }
        public string ReadingProgressDisplay { get; set; }
        public DateTime? StartedReadingAt { get; set; }
        public DateTime? FinishedReadingAt { get; set; }
        public int? UserRating { get; set; }
        public string UserNotes { get; set; }
        public string UserReview { get; set; }
        public bool? IsFavorite { get; set; }
        public string SeriesName { get; set; }
        public int? SeriesPosition { get; set; }
        public string CollectionName { get; set; }
        public string Format { get; set; }
        public string Source { get; set; }
    }

    public class GoogleBookDataDto
    {
        public string ApiTitle { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public int? PageCount { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public string PreviewLink { get; set; }
        public string InfoLink { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<string> ApiAuthors { get; set; }
        public List<string> ApiCategories { get; set; }
        public string Language { get; set; }
        public double? AverageRating { get; set; }
        public int? RatingsCount { get; set; }
    }
}
