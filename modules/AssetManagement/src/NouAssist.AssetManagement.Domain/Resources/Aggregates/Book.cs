using System;
using System.Collections.Generic;
using NouAssist.AssetManagement.Resources.Enums;
using NouAssist.AssetManagement.Resources.Events;
using NouAssist.AssetManagement.Resources.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.AssetManagement.Resources.Aggregates
{
    public class Book : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public const int MaxTitleLength = 500;

        public Guid OwnerId { get; private set; }
        public Guid ResourceId { get; private set; }
        public string Title { get; private set; }
        public List<string> Authors { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string Publisher { get; private set; }
        public string PublishedDate { get; private set; }
        public string Edition { get; private set; }
        public string Language { get; private set; }
        public int? PageCount { get; private set; }
        public string Isbn10 { get; private set; }
        public string Isbn13 { get; private set; }
        public string Asin { get; private set; }
        public string Doi { get; private set; }
        public Guid? CoverImageResourceId { get; private set; }
        public GoogleBookData GoogleBookData { get; private set; }
        public List<string> Categories { get; private set; }
        public List<string> Tags { get; private set; }
        public ReadingStatus ReadingStatus { get; private set; }
        public int? CurrentPage { get; private set; }
        public double? ProgressPercentage { get; private set; }
        public DateTime? StartedReadingAt { get; private set; }
        public DateTime? FinishedReadingAt { get; private set; }
        public int? UserRating { get; private set; }
        public string UserNotes { get; private set; }
        public string UserReview { get; private set; }
        public bool? IsFavorite { get; private set; }
        public string SeriesName { get; private set; }
        public int? SeriesPosition { get; private set; }
        public string CollectionName { get; private set; }
        public BookFormat Format { get; private set; }
        public BookSource Source { get; private set; }
        public Dictionary<string, object> CustomFields { get; private set; }
        public Guid? TenantId { get; private set; }

        protected Book() { /* ORM */ }

        public Book(
            Guid id,
            Guid ownerId,
            Guid resourceId,
            string title,
            Guid? tenantId,
            List<string> authors = null,
            BookFormat format = BookFormat.Pdf) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            ResourceId = Check.NotNull(resourceId, nameof(resourceId));
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), MaxTitleLength);
            TenantId = tenantId;
            Authors = authors ?? new List<string>();
            Format = format;
            Source = BookSource.UserUploaded;
            ReadingStatus = ReadingStatus.Unread;
            
            Categories = new List<string>();
            Tags = new List<string>();
            CustomFields = new Dictionary<string, object>();
        }

        public void UpdateGoogleBookData(GoogleBookData data)
        {
            GoogleBookData = Check.NotNull(data, nameof(data));
            
            // Merge data if user hasn't set it
            if (Authors.Count == 0 && data.ApiAuthors != null && data.ApiAuthors.Count > 0)
            {
                Authors = new List<string>(data.ApiAuthors);
            }
            
            Publisher ??= data.Publisher;
            PublishedDate ??= data.PublishedDate;
            Description ??= data.Description;
            PageCount ??= data.PageCount;
            Isbn10 ??= data.Isbn10;
            Isbn13 ??= data.Isbn13;
            Language ??= data.Language;
            
            if (Categories.Count == 0 && data.ApiCategories != null && data.ApiCategories.Count > 0)
            {
                Categories = new List<string>(data.ApiCategories);
            }
        }

        public void StartReading()
        {
            if (ReadingStatus == ReadingStatus.Reading)
            {
                return;
            }

            ReadingStatus = ReadingStatus.Reading;
            StartedReadingAt ??= DateTime.UtcNow;
        }

        public void UpdateProgress(int? page = null, double? percentage = null)
        {
            if (page.HasValue)
            {
                CurrentPage = page.Value;
                
                if (PageCount.HasValue && PageCount.Value > 0)
                {
                    ProgressPercentage = Math.Clamp((double)page.Value / PageCount.Value * 100, 0, 100);
                }
            }

            if (percentage.HasValue)
            {
                ProgressPercentage = Math.Clamp(percentage.Value, 0, 100);
            }

            // Auto-start reading if not already
            if (ReadingStatus == ReadingStatus.Unread)
            {
                StartReading();
            }

            // Publish progress event
            AddDistributedEvent(new BookReadingProgressUpdatedEto
            {
                BookId = Id,
                OwnerId = OwnerId,
                ReadingStatus = ReadingStatus.ToString(),
                ProgressPercentage = ProgressPercentage,
                CurrentPage = CurrentPage,
                TenantId = TenantId,
                UpdatedAt = DateTime.UtcNow
            });
        }

        public void FinishReading()
        {
            ReadingStatus = ReadingStatus.Finished;
            FinishedReadingAt = DateTime.UtcNow;
            ProgressPercentage = 100.0;
            
            if (PageCount.HasValue && PageCount.Value > 0)
            {
                CurrentPage = PageCount.Value;
            }

            // Publish progress event
            AddDistributedEvent(new BookReadingProgressUpdatedEto
            {
                BookId = Id,
                OwnerId = OwnerId,
                ReadingStatus = ReadingStatus.ToString(),
                ProgressPercentage = 100.0,
                CurrentPage = CurrentPage,
                TenantId = TenantId,
                UpdatedAt = DateTime.UtcNow
            });
        }

        public void MarkAsAbandoned()
        {
            ReadingStatus = ReadingStatus.Abandoned;
        }

        public void ResetProgress()
        {
            ReadingStatus = ReadingStatus.Unread;
            CurrentPage = null;
            ProgressPercentage = null;
            StartedReadingAt = null;
            FinishedReadingAt = null;
        }

        public void Rate(int rating)
        {
            if (rating < 1 || rating > 5)
            {
                throw new BusinessException("Books:InvalidRating", "Rating must be between 1 and 5.");
            }

            UserRating = rating;
        }

        public void AddToFavorites()
        {
            IsFavorite = true;
        }

        public void RemoveFromFavorites()
        {
            IsFavorite = false;
        }

        public void UpdateMetadata(
            string title = null,
            List<string> authors = null,
            string subtitle = null,
            string description = null,
            string publisher = null,
            List<string> categories = null,
            List<string> tags = null)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                Title = Check.Length(title, nameof(title), MaxTitleLength);
            }

            if (authors != null)
            {
                Authors = new List<string>(authors);
            }

            Subtitle = subtitle ?? Subtitle;
            Description = description ?? Description;
            Publisher = publisher ?? Publisher;

            if (categories != null)
            {
                Categories = new List<string>(categories);
            }

            if (tags != null)
            {
                Tags = new List<string>(tags);
            }
        }

        public void AddTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return;
            }

            if (Tags.Contains(tag))
            {
                return;
            }

            Tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
        }

        public void AddToSeries(string seriesName, int? position = null)
        {
            SeriesName = Check.NotNullOrWhiteSpace(seriesName, nameof(seriesName));
            SeriesPosition = position;
        }

        public void AddToCollection(string collectionName)
        {
            CollectionName = Check.NotNullOrWhiteSpace(collectionName, nameof(collectionName));
        }

        public void SetCoverImage(Guid coverImageResourceId)
        {
            CoverImageResourceId = coverImageResourceId;
        }

        public void SetReview(string review)
        {
            UserReview = review;
        }

        public void SetNotes(string notes)
        {
            UserNotes = notes;
        }

        // Computed properties
        public bool HasAuthors => Authors != null && Authors.Count > 0;
        public bool HasGoogleBookData => GoogleBookData != null;
        public bool HasCoverImage => CoverImageResourceId.HasValue;
        public bool HasIsbn => !string.IsNullOrWhiteSpace(Isbn10) || !string.IsNullOrWhiteSpace(Isbn13);
        public bool HasCategories => Categories != null && Categories.Count > 0;
        public bool HasTags => Tags != null && Tags.Count > 0;
        public bool HasUserRating => UserRating.HasValue && UserRating.Value > 0;
        public bool IsInSeries => !string.IsNullOrWhiteSpace(SeriesName);
        public bool IsInCollection => !string.IsNullOrWhiteSpace(CollectionName);
        public bool HasReadingProgress => CurrentPage.HasValue || ProgressPercentage.HasValue;
        public bool IsUnread => ReadingStatus == ReadingStatus.Unread;
        public bool IsReading => ReadingStatus == ReadingStatus.Reading;
        public bool IsFinished => ReadingStatus == ReadingStatus.Finished;
        public bool IsAbandoned => ReadingStatus == ReadingStatus.Abandoned;

        public string GetDisplayAuthors()
        {
            if (HasAuthors)
            {
                if (Authors.Count == 1)
                {
                    return Authors[0];
                }

                if (Authors.Count == 2)
                {
                    return string.Join(" and ", Authors);
                }

                return $"{Authors[0]} et al.";
            }

            if (HasGoogleBookData && GoogleBookData.ApiAuthors != null && GoogleBookData.ApiAuthors.Count > 0)
            {
                return GoogleBookData.GetDisplayAuthors();
            }

            return "Unknown Author";
        }

        public string GetPrimaryIsbn()
        {
            if (!string.IsNullOrWhiteSpace(Isbn13))
            {
                return Isbn13;
            }

            if (!string.IsNullOrWhiteSpace(Isbn10))
            {
                return Isbn10;
            }

            if (HasGoogleBookData)
            {
                return GoogleBookData.PrimaryIsbn;
            }

            return string.Empty;
        }

        public string GetDisplayPublisher()
        {
            if (!string.IsNullOrWhiteSpace(Publisher))
            {
                return Publisher;
            }

            if (HasGoogleBookData && !string.IsNullOrWhiteSpace(GoogleBookData.Publisher))
            {
                return GoogleBookData.Publisher;
            }
            return "Unknown Publisher";
        }

        public int GetDisplayPageCount()
        {
            if (PageCount.HasValue)
            {
                return PageCount.Value;
            }

            if (HasGoogleBookData && GoogleBookData.PageCount.HasValue)
            {
                return GoogleBookData.PageCount.Value;
            }
            return 0;
        }

        public string GetReadingProgressDisplay()
        {
            if (ProgressPercentage.HasValue)
            {
                return $"{ProgressPercentage.Value:F1}%";
            }

            var displayPageCount = GetDisplayPageCount();
            if (CurrentPage.HasValue && displayPageCount > 0)
            {
                var percentage = (double)CurrentPage.Value / displayPageCount * 100;
                return $"{percentage:F1}%";
            }

            return "0%";
        }

        public void Delete(string deletedBy)
        {
            IsDeleted = true;
            DeleterId = Guid.TryParse(deletedBy, out var id) ? id : null;
            DeletionTime = DateTime.UtcNow;
        }
    }
}
