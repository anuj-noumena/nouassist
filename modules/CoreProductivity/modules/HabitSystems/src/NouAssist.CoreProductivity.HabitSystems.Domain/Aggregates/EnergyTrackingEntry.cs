using System;
using NouAssist.CoreProductivity.HabitSystems.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CoreProductivity.HabitSystems.Aggregates
{
    public class EnergyTrackingEntry : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public DateTime Date { get; private set; }
        public int EnergyLevel { get; private set; }
        public string Note { get; set; }
        public Guid? TenantId { get; private set; }

        protected EnergyTrackingEntry() { /* ORM */ }

        public EnergyTrackingEntry(
            Guid id,
            Guid ownerId,
            DateTime date,
            int energyLevel,
            Guid? tenantId = null
        ) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Date = new DateTime(date.Year, date.Month, date.Day);
            SetEnergyLevel(energyLevel);
            TenantId = tenantId;
        }

        public EnergyLevel Level
        {
            get
            {
                if (EnergyLevel >= 8) return Enums.EnergyLevel.High;
                if (EnergyLevel >= 5) return Enums.EnergyLevel.Medium;
                return Enums.EnergyLevel.Low;
            }
        }

        public void SetEnergyLevel(int level)
        {
            if (level < 1 || level > 10)
            {
                throw new BusinessException("Habits:InvalidEnergyLevel", 
                    "Energy level must be between 1 and 10");
            }
            EnergyLevel = level;
        }

        public void AddNote(string note)
        {
            Note = note;
        }
    }
}
