using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.LearningDevelopment.Integration.LearningIntegration.Dtos
{
    public class CreateIntegratedLearningPathDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public Guid TopicId { get; set; }

        [Required]
        public Guid SkillId { get; set; }

        public bool CreateTasks { get; set; } = true;
        public bool ScheduleTimeblocks { get; set; } = true;

        public DateTime? StartDate { get; set; }
        public List<DayOfWeek> PreferredDays { get; set; }
        public TimeSpan? PreferredStartTime { get; set; }
        public int? SessionDurationMinutes { get; set; }
        public int EstimatedHours { get; set; } = 40;
    }
}
