using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.Projects.ValueObjects
{
    public class DateRange : ValueObject
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        private DateRange() { }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new BusinessException("Projects:InvalidDateRange", 
                    "End date must be after start date.");
            }

            StartDate = startDate;
            EndDate = endDate;
        }

        public int DurationInDays => (EndDate - StartDate).Days;
        public TimeSpan Duration => EndDate - StartDate;
        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
        public bool IsFuture => DateTime.UtcNow < StartDate;
        public bool IsPast => DateTime.UtcNow > EndDate;

        public TimeSpan? TimeRemaining
        {
            get
            {
                if (IsPast)
                {
                    return null;
                }

                return EndDate - DateTime.UtcNow;
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StartDate;
            yield return EndDate;
        }
    }
}
