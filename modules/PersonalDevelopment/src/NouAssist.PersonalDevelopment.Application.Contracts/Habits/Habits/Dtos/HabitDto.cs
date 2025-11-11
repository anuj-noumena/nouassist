using System;
using System.Collections.Generic;
using NouAssist.PersonalDevelopment.Habits.Enums;
using Volo.Abp.Application.Dtos;
using DayOfWeek = System.DayOfWeek;

namespace NouAssist.PersonalDevelopment.Habits.Habits.Dtos
{
    public class HabitDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Goal { get; set; }
        public string Description { get; set; }
        public HabitType HabitType { get; set; }
        public bool IsActive { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? ContextId { get; set; }
        public List<DayOfWeek> ScheduledDays { get; set; }
        public List<HabitCueDto> Cues { get; set; }
        public List<HabitResponseDto> Responses { get; set; }
        public List<HabitRewardDto> Rewards { get; set; }
        public List<HabitStrategyDto> Strategies { get; set; }

        // Computed properties
        public bool IsMakeHabit { get; set; }
        public bool IsBreakHabit { get; set; }
        public bool HasSchedule { get; set; }
        public bool IsScheduledForToday { get; set; }
    }

    public class HabitCueDto : EntityDto<Guid>
    {
        public CueType CueType { get; set; }
        public string Description { get; set; }
        public string CueValue { get; set; }
        public string CueTypeDescription { get; set; }
    }

    public class HabitResponseDto : EntityDto<Guid>
    {
        public string Description { get; set; }
        public string Details { get; set; }
        public int? DurationMinutes { get; set; }
        public bool HasDuration { get; set; }
    }

    public class HabitRewardDto : EntityDto<Guid>
    {
        public string Description { get; set; }
        public string Craving { get; set; }
        public bool HasCraving { get; set; }
    }

    public class HabitStrategyDto : EntityDto<Guid>
    {
        public HabitStrategyType StrategyType { get; set; }
        public string Description { get; set; }
        public string StrategyTypeDescription { get; set; }
    }
}
