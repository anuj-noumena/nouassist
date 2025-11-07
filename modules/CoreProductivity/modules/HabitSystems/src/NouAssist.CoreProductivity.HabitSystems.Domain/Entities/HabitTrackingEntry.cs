using System;
using NouAssist.CoreProductivity.HabitSystems.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CoreProductivity.HabitSystems.Entities
{
    public class HabitTrackingEntry : CreationAuditedEntity<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid HabitId { get; private set; }
        public DateTime LogDate { get; private set; }
        public bool Completed { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public string Notes { get; set; }
        public Guid? TenantId { get; private set; }

        protected HabitTrackingEntry() { /* ORM */ }

        public HabitTrackingEntry(
            Guid id,
            Guid ownerId,
            Guid habitId,
            DateTime logDate,
            bool completed = false,
            Guid? tenantId = null
        )
        {
            Id = id;
            OwnerId = ownerId;
            HabitId = habitId;
            LogDate = NormalizeDate(logDate);
            Completed = completed;
            CompletedAt = completed ? DateTime.UtcNow : null;
            TenantId = tenantId;
        }

        public void MarkAsCompleted()
        {
            if (Completed) return;

            Completed = true;
            CompletedAt = DateTime.UtcNow;

            AddDistributedEvent(new HabitCompletedEto
            {
                HabitId = HabitId,
                TenantId = TenantId,
                OccurredAt = CompletedAt.Value,
                LogDate = LogDate,
                CurrentStreak = 0 // Will be calculated by domain service
            });
        }

        public void MarkAsIncomplete()
        {
            if (!Completed) return;

            Completed = false;
            CompletedAt = null;
        }

        public void AddNotes(string notes)
        {
            Notes = notes;
        }

        private static DateTime NormalizeDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
    }
}
