using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CoreProductivity.HabitSystems.Aggregates
{
    public class DailyQuote : AggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public string Author { get; set; }
        public DateTime? ViewedAt { get; private set; }
        public Guid? TenantId { get; private set; }

        protected DailyQuote() { /* ORM */ }

        public DailyQuote(
            Guid id,
            Guid ownerId,
            DateTime date,
            string description,
            string author = null,
            Guid? tenantId = null
        ) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Date = new DateTime(date.Year, date.Month, date.Day);
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 500);
            Author = author;
            TenantId = tenantId;
        }

        public bool HasBeenViewed => ViewedAt.HasValue;
        public bool HasAuthor => !string.IsNullOrWhiteSpace(Author);

        public void MarkAsViewed()
        {
            if (HasBeenViewed) return;
            ViewedAt = DateTime.UtcNow;
        }
    }
}
