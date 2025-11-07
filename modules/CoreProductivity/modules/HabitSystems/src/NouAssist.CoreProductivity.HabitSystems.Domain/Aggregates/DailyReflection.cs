using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CoreProductivity.HabitSystems.Aggregates
{
    public class DailyReflection : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public DateTime Date { get; private set; }
        public int? Rating { get; private set; }
        public string WentWell { get; set; }
        public string ToImprove { get; set; }
        public string Learned { get; set; }
        public string Gratitude { get; set; }
        public string TomorrowPriorities { get; set; }
        public Guid? TenantId { get; private set; }

        protected DailyReflection() { /* ORM */ }

        public DailyReflection(Guid id, Guid ownerId, DateTime date, Guid? tenantId = null) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Date = new DateTime(date.Year, date.Month, date.Day);
            TenantId = tenantId;
        }

        public bool IsComplete => Rating.HasValue;
        public bool HasGratitude => !string.IsNullOrWhiteSpace(Gratitude);
        public bool HasPriorities => !string.IsNullOrWhiteSpace(TomorrowPriorities);

        public void SetRating(int rating)
        {
            if (rating < 1 || rating > 10)
            {
                throw new BusinessException("Habits:InvalidRating", 
                    "Rating must be between 1 and 10");
            }
            Rating = rating;
        }

        public void UpdateReflections(
            string wentWell = null,
            string toImprove = null,
            string learned = null,
            string gratitude = null
        )
        {
            WentWell = wentWell ?? WentWell;
            ToImprove = toImprove ?? ToImprove;
            Learned = learned ?? Learned;
            Gratitude = gratitude ?? Gratitude;
        }

        public void SetTomorrowPriorities(string priorities)
        {
            TomorrowPriorities = priorities;
        }
    }
}
