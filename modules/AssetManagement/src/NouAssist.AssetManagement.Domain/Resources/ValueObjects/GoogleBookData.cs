using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Resources.ValueObjects
{
    public class GoogleBookData : ValueObject
    {
        public string ApiTitle { get; init; }
        public string Publisher { get; init; }
        public string PublishedDate { get; init; }
        public string Description { get; init; }
        public int? PageCount { get; init; }
        public string Isbn10 { get; init; }
        public string Isbn13 { get; init; }
        public string PreviewLink { get; init; }
        public string InfoLink { get; init; }
        public string ThumbnailUrl { get; init; }
        public string SmallThumbnailUrl { get; init; }
        public List<string> ApiAuthors { get; init; }
        public List<string> ApiCategories { get; init; }
        public string Language { get; init; }
        public double? AverageRating { get; init; }
        public int? RatingsCount { get; init; }
        public string MaturityRating { get; init; }
        public Dictionary<string, object> RawData { get; init; }
        public DateTime FetchedAt { get; init; }

        public GoogleBookData(
            string apiTitle,
            DateTime fetchedAt,
            string publisher = null,
            string publishedDate = null,
            string description = null,
            int? pageCount = null,
            string isbn10 = null,
            string isbn13 = null,
            string previewLink = null,
            string infoLink = null,
            string thumbnailUrl = null,
            string smallThumbnailUrl = null,
            List<string> apiAuthors = null,
            List<string> apiCategories = null,
            string language = null,
            double? averageRating = null,
            int? ratingsCount = null,
            string maturityRating = null,
            Dictionary<string, object> rawData = null)
        {
            ApiTitle = apiTitle;
            FetchedAt = fetchedAt;
            Publisher = publisher;
            PublishedDate = publishedDate;
            Description = description;
            PageCount = pageCount;
            Isbn10 = isbn10;
            Isbn13 = isbn13;
            PreviewLink = previewLink;
            InfoLink = infoLink;
            ThumbnailUrl = thumbnailUrl;
            SmallThumbnailUrl = smallThumbnailUrl;
            ApiAuthors = apiAuthors ?? new List<string>();
            ApiCategories = apiCategories ?? new List<string>();
            Language = language;
            AverageRating = averageRating;
            RatingsCount = ratingsCount;
            MaturityRating = maturityRating;
            RawData = rawData;
        }

        public bool HasIsbn => !string.IsNullOrWhiteSpace(Isbn10) || !string.IsNullOrWhiteSpace(Isbn13);
        public bool HasRating => AverageRating.HasValue && AverageRating.Value > 0;
        public bool HasPreview => !string.IsNullOrWhiteSpace(PreviewLink);
        public string PrimaryIsbn => Isbn13 ?? Isbn10 ?? string.Empty;

        public string GetDisplayAuthors()
        {
            if (ApiAuthors == null || ApiAuthors.Count == 0) return "Unknown Author";
            if (ApiAuthors.Count == 1) return ApiAuthors[0];
            if (ApiAuthors.Count == 2) return string.Join(" and ", ApiAuthors);
            return $"{ApiAuthors[0]} et al.";
        }

        public string GetDisplayCategories()
        {
            if (ApiCategories == null || ApiCategories.Count == 0) return "Uncategorized";
            return string.Join(", ", ApiCategories.GetRange(0, Math.Min(3, ApiCategories.Count)));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ApiTitle;
            yield return Publisher;
            yield return PublishedDate;
            yield return PageCount;
            yield return Isbn10;
            yield return Isbn13;
            yield return FetchedAt;
        }
    }
}
