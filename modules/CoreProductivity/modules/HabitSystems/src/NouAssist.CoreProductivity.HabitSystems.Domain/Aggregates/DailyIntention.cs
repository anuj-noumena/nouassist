using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CoreProductivity.HabitSystems.Aggregates
{
    public class DailyIntention : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public DateTime Date { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<string> _intentions = new();
        public IReadOnlyCollection<string> Intentions => _intentions.AsReadOnly();

        protected DailyIntention() { /* ORM */ }

        public DailyIntention(Guid id, Guid ownerId, DateTime date, Guid? tenantId = null) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Date = new DateTime(date.Year, date.Month, date.Day);
            TenantId = tenantId;
        }

        public bool HasIntentions => _intentions.Any();
        public int IntentionCount => _intentions.Count;

        public void AddIntention(string intention)
        {
            if (string.IsNullOrWhiteSpace(intention))
            {
                throw new BusinessException("Habits:EmptyIntention", 
                    "Intention cannot be empty");
            }

            if (_intentions.Count >= 10)
            {
                throw new BusinessException("Habits:MaxIntentionsReached", 
                    "Maximum 10 intentions allowed per day");
            }

            _intentions.Add(intention);
        }

        public void RemoveIntention(int index)
        {
            if (index < 0 || index >= _intentions.Count)
            {
                throw new BusinessException("Habits:InvalidIndex", 
                    "Invalid intention index");
            }

            _intentions.RemoveAt(index);
        }

        public void UpdateIntention(int index, string newIntention)
        {
            if (index < 0 || index >= _intentions.Count)
            {
                throw new BusinessException("Habits:InvalidIndex", 
                    "Invalid intention index");
            }

            if (string.IsNullOrWhiteSpace(newIntention))
            {
                throw new BusinessException("Habits:EmptyIntention", 
                    "Intention cannot be empty");
            }

            _intentions[index] = newIntention;
        }

        public void ClearIntentions()
        {
            _intentions.Clear();
        }
    }
}
