using System;
using NouAssist.Calendar.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Calendar.Entities
{
    public class CalendarEventException : Entity<Guid>
    {
        public Guid EventId { get; private set; }
        public DateTime InstanceDate { get; private set; }
        public DateTime? OriginalStart { get; private set; }
        public DateTime? OriginalEnd { get; private set; }
        public DateTime? ModifiedStart { get; private set; }
        public DateTime? ModifiedEnd { get; private set; }
        public string ModifiedTitle { get; private set; }
        public string ModifiedDescription { get; private set; }
        public string ModifiedLocation { get; private set; }
        public string ModifiedNotes { get; private set; }
        public bool IsCancelled { get; private set; }
        public DateTime? CancelledAt { get; private set; }
        public string CancelledBy { get; private set; }
        public string CancellationReason { get; private set; }
        public string ModifiedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected CalendarEventException() { }

        public CalendarEventException(
            Guid id,
            Guid eventId,
            DateTime instanceDate,
            DateTime originalStart,
            DateTime originalEnd)
            : base(id)
        {
            EventId = eventId;
            InstanceDate = NormalizeDate(instanceDate);
            OriginalStart = originalStart;
            OriginalEnd = originalEnd;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reschedule(DateTime newStart, DateTime newEnd, string modifiedBy)
        {
            if (newStart >= newEnd)
                throw new BusinessException("Calendar:InvalidTimeRange");

            ModifiedStart = newStart;
            ModifiedEnd = newEnd;
            ModifiedBy = modifiedBy;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateContent(string title = null, string description = null, 
            string location = null, string notes = null, string modifiedBy = null)
        {
            if (title != null) ModifiedTitle = title;
            if (description != null) ModifiedDescription = description;
            if (location != null) ModifiedLocation = location;
            if (notes != null) ModifiedNotes = notes;
            if (modifiedBy != null) ModifiedBy = modifiedBy;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel(string cancelledBy, string cancellationReason = null)
        {
            if (IsCancelled)
                return;

            IsCancelled = true;
            CancelledAt = DateTime.UtcNow;
            CancelledBy = Check.NotNullOrWhiteSpace(cancelledBy, nameof(cancelledBy));
            CancellationReason = cancellationReason;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            if (!IsCancelled)
                return;

            IsCancelled = false;
            CancelledAt = null;
            CancelledBy = null;
            CancellationReason = null;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsModified => ModifiedStart.HasValue || ModifiedEnd.HasValue ||
                                 !string.IsNullOrEmpty(ModifiedTitle) ||
                                 !string.IsNullOrEmpty(ModifiedDescription) ||
                                 !string.IsNullOrEmpty(ModifiedLocation) ||
                                 !string.IsNullOrEmpty(ModifiedNotes);

        public DateTime EffectiveStart => ModifiedStart ?? OriginalStart.Value;
        public DateTime EffectiveEnd => ModifiedEnd ?? OriginalEnd.Value;

        public InstanceStatus Status
        {
            get
            {
                if (IsCancelled) return InstanceStatus.Cancelled;
                if (IsModified) return InstanceStatus.Modified;
                return InstanceStatus.Normal;
            }
        }

        private static DateTime NormalizeDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
    }
}
