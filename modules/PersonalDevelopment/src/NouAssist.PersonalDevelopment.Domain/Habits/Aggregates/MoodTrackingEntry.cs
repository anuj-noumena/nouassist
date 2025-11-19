using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.PersonalDevelopment.Habits.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.PersonalDevelopment.Habits.Aggregates
{
    public class MoodTrackingEntry : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public DateTime Date { get; private set; }
        public int MoodRating { get; private set; }
        public string Note { get; set; }
        public Guid? TenantId { get; private set; }

        private readonly List<string> _tags = new();
        public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

        protected MoodTrackingEntry() { /* ORM */ }

        public MoodTrackingEntry(
            Guid id,
            Guid ownerId,
            DateTime date,
            int moodRating,
            Guid? tenantId = null
        ) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Date = new DateTime(date.Year, date.Month, date.Day);
            SetMoodRating(moodRating);
            TenantId = tenantId;
        }

        public MoodLevel MoodLevel
        {
            get
            {
                if (MoodRating >= 8)
                {
                    return MoodLevel.VeryGood;
                }

                if (MoodRating >= 6)
                {
                    return MoodLevel.Good;
                }

                if (MoodRating >= 4)
                {
                    return MoodLevel.Neutral;
                }

                if (MoodRating >= 2)
                {
                    return MoodLevel.Poor;
                }

                return MoodLevel.VeryPoor;
            }
        }

        public void SetMoodRating(int rating)
        {
            if (rating < 1 || rating > 10)
            {
                throw new BusinessException("Habits:InvalidMoodRating", 
                    "Mood rating must be between 1 and 10");
            }
            MoodRating = rating;
        }

        public void AddNote(string note)
        {
            Note = note;
        }

        public void AddTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                throw new BusinessException("Habits:InvalidTag", "Tag cannot be empty");
            }

            if (_tags.Contains(tag, StringComparer.OrdinalIgnoreCase))
            {
                return; // Already exists
            }

            if (_tags.Count >= 10)
            {
                throw new BusinessException("Habits:MaxTagsReached", 
                    "Maximum 10 tags allowed");
            }

            _tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            _tags.Remove(tag);
        }
    }
}
