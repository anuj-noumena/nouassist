using System;
using NouAssist.Habits.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Habits.Dtos
{
    public class GetHabitListDto : PagedAndSortedResultRequestDto
    {
        public HabitType? HabitType { get; set; }
        public bool? IsActive { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? ContextId { get; set; }
        public DateTime? ScheduledForDate { get; set; }
    }
}
