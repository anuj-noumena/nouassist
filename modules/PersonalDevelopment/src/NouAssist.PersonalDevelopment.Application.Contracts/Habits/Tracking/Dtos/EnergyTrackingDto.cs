using System;
using NouAssist.PersonalDevelopment.Habits.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.PersonalDevelopment.Habits.Tracking.Dtos
{
    public class EnergyTrackingDto : AuditedEntityDto<Guid>
    {
        public DateTime Date { get; set; }
        public int EnergyLevel { get; set; }
        public string Note { get; set; }
        public EnergyLevel Level { get; set; }
    }

    public class UpdateEnergyTrackingDto
    {
        public DateTime Date { get; set; }
        public int EnergyLevel { get; set; }
        public string Note { get; set; }
    }
}
