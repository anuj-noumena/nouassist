using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CoreProductivity.HabitSystems.Planning.Dtos
{
    public class DailyIntentionDto : AuditedEntityDto<Guid>
    {
        public DateTime Date { get; set; }
        public List<string> Intentions { get; set; }
        public bool HasIntentions { get; set; }
        public int IntentionCount { get; set; }
    }

    public class UpdateDailyIntentionDto
    {
        public DateTime Date { get; set; }
        public List<string> Intentions { get; set; }
    }
}
