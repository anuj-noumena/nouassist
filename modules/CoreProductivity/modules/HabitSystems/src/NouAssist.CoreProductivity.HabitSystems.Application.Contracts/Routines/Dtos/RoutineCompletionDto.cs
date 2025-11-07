using System;
using System.Collections.Generic;
using NouAssist.CoreProductivity.HabitSystems.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CoreProductivity.HabitSystems.Routines.Dtos
{
    public class RoutineCompletionDto : EntityDto<Guid>
    {
        public Guid RoutineId { get; set; }
        public DateTime LogDate { get; set; }
        public RoutineCompletionStatus Status { get; set; }
        public string Notes { get; set; }
        public int Completed { get; set; }
        public int? TotalSteps { get; set; }
        public double CompletionRate { get; set; }
        public List<Guid> CompletedStepIds { get; set; }
    }

    public class CompleteRoutineDto
    {
        public DateTime LogDate { get; set; }
        public List<Guid> CompletedStepIds { get; set; } = new();
        public string Notes { get; set; }
    }

    public class GetRoutineListDto : PagedAndSortedResultRequestDto
    {
        public bool? IsActive { get; set; }
        public string TimeOfDay { get; set; }
        public DateTime? ScheduledForDate { get; set; }
    }
}
