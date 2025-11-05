using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Habits.Routines.Dtos
{
    public class RoutineDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TriggerDescription { get; set; }
        public string TimeOfDay { get; set; }
        public bool IsActive { get; set; }
        public List<int> ScheduledDays { get; set; }
        public List<RoutineStepDto> Steps { get; set; }

        public bool HasSchedule { get; set; }
        public bool IsEveryday { get; set; }
        public int TotalSteps { get; set; }
        public int EstimatedDurationMinutes { get; set; }
    }

    public class RoutineStepDto : EntityDto<Guid>
    {
        public int StepNumber { get; set; }
        public Guid? HabitId { get; set; }
        public string HabitTitle { get; set; }
        public string AdhocTaskDescription { get; set; }
        public int? EstimatedDurationMinutes { get; set; }
        public bool IsHabitStep { get; set; }
        public bool IsAdhocStep { get; set; }
    }
}
