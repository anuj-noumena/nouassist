using System;
using System.Collections.Generic;
using NouAssist.Habits.Dtos;
using NouAssist.Habits.Routines.Dtos;

namespace NouAssist.Habits.Planning.Dtos
{
    public class DailyPlanDto
    {
        public DateTime Date { get; set; }
        public DailyIntentionDto Intention { get; set; }
        public DailyQuoteDto Quote { get; set; }
        public List<HabitDto> ScheduledHabits { get; set; }
        public List<RoutineDto> ScheduledRoutines { get; set; }
        public bool HasIntention { get; set; }
        public bool HasPreviousReflection { get; set; }
        public int EstimatedFocusMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetDailyPlanDto
    {
        public DateTime Date { get; set; }
    }
}
