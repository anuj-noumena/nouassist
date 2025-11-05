using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Habits.Planning.Dtos
{
    public class DailyReflectionDto : AuditedEntityDto<Guid>
    {
        public DateTime Date { get; set; }
        public int? Rating { get; set; }
        public string WentWell { get; set; }
        public string ToImprove { get; set; }
        public string Learned { get; set; }
        public string Gratitude { get; set; }
        public string TomorrowPriorities { get; set; }
        
        public bool IsComplete { get; set; }
        public bool HasGratitude { get; set; }
        public bool HasPriorities { get; set; }
    }

    public class UpdateDailyReflectionDto
    {
        public DateTime Date { get; set; }
        public int? Rating { get; set; }
        public string WentWell { get; set; }
        public string ToImprove { get; set; }
        public string Learned { get; set; }
        public string Gratitude { get; set; }
        public string TomorrowPriorities { get; set; }
    }
}
