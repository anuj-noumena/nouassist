using System;
using NouAssist.PersonalDevelopment.Habits.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.PersonalDevelopment.Habits.Habits.Dtos
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
