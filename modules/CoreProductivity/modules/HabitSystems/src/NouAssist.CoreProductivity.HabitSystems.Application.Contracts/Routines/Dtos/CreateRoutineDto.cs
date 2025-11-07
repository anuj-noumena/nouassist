using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.CoreProductivity.HabitSystems.Routines.Dtos
{
    public class CreateRoutineDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(500)]
        public string TriggerDescription { get; set; }

        [StringLength(50)]
        public string TimeOfDay { get; set; }

        public List<int> ScheduledDays { get; set; } = new();
    }

    public class UpdateRoutineDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(500)]
        public string TriggerDescription { get; set; }

        [StringLength(50)]
        public string TimeOfDay { get; set; }

        public List<int> ScheduledDays { get; set; } = new();
    }
}
