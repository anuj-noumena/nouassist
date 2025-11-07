using System;
using System.Collections.Generic;
using NouAssist.CoreProductivity.HabitSystems.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CoreProductivity.HabitSystems.Tracking.Dtos
{
    public class MoodTrackingDto : AuditedEntityDto<Guid>
    {
        public DateTime Date { get; set; }
        public int MoodRating { get; set; }
        public string Note { get; set; }
        public List<string> Tags { get; set; }
        public MoodLevel MoodLevel { get; set; }
    }

    public class UpdateMoodTrackingDto
    {
        public DateTime Date { get; set; }
        public int MoodRating { get; set; }
        public string Note { get; set; }
        public List<string> Tags { get; set; }
    }
}
