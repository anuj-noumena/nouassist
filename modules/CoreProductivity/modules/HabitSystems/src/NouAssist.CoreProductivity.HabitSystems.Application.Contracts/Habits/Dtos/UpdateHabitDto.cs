using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.CoreProductivity.HabitSystems.Enums;

namespace NouAssist.CoreProductivity.HabitSystems.Dtos
{
    public class UpdateHabitDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Goal { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public Guid? AreaId { get; set; }
        public Guid? ContextId { get; set; }

        public List<DayOfWeek> ScheduledDays { get; set; } = new();
    }
}
