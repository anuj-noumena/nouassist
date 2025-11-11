using AutoMapper;

namespace NouAssist.PersonalDevelopment.HabitSystems
{
    public class HabitsApplicationAutoMapperProfile : Profile
    {
        public HabitsApplicationAutoMapperProfile()
        {
            // Habit mappings
            CreateMap<Habit, HabitDto>()
                .ForMember(d => d.IsMakeHabit, opt => opt.MapFrom(s => s.IsMakeHabit))
                .ForMember(d => d.IsBreakHabit, opt => opt.MapFrom(s => s.IsBreakHabit))
                .ForMember(d => d.HasSchedule, opt => opt.MapFrom(s => s.HasSchedule))
                .ForMember(d => d.IsScheduledForToday, opt => opt.MapFrom(s => s.IsScheduledForToday()));

            CreateMap<HabitCue, HabitCueDto>()
                .ForMember(d => d.CueTypeDescription, opt => opt.MapFrom(s => s.GetCueTypeDescription()));

            CreateMap<HabitResponse, HabitResponseDto>()
                .ForMember(d => d.HasDuration, opt => opt.MapFrom(s => s.HasDuration));

            CreateMap<HabitReward, HabitRewardDto>()
                .ForMember(d => d.HasCraving, opt => opt.MapFrom(s => s.HasCraving));

            CreateMap<HabitStrategy, HabitStrategyDto>()
                .ForMember(d => d.StrategyTypeDescription, opt => opt.MapFrom(s => s.GetStrategyTypeDescription()));

            CreateMap<HabitTrackingEntry, HabitTrackingDto>();

            CreateMap<HabitMetrics, HabitMetricsDto>();

            // Routine mappings
            CreateMap<Routine, RoutineDto>()
                .ForMember(d => d.HasSchedule, opt => opt.MapFrom(s => s.HasSchedule))
                .ForMember(d => d.IsEveryday, opt => opt.MapFrom(s => s.IsEveryday))
                .ForMember(d => d.TotalSteps, opt => opt.MapFrom(s => s.TotalSteps))
                .ForMember(d => d.EstimatedDurationMinutes, opt => opt.Ignore()); // Calculated in service

            CreateMap<RoutineStep, RoutineStepDto>()
                .ForMember(d => d.HabitTitle, opt => opt.Ignore()) // Resolved in service
                .ForMember(d => d.IsHabitStep, opt => opt.MapFrom(s => s.IsHabitStep))
                .ForMember(d => d.IsAdhocStep, opt => opt.MapFrom(s => s.IsAdhocStep));

            CreateMap<RoutineCompletion, RoutineCompletionDto>()
                .ForMember(d => d.CompletionRate, opt => opt.MapFrom(s => s.CompletionRate))
                .ForMember(d => d.CompletedStepIds, opt => opt.MapFrom(s => s.CompletedSteps.Select(cs => cs.StepId).ToList()));

            // Tracking mappings
            CreateMap<HealthTrackingEntry, HealthTrackingDto>()
                .ForMember(d => d.HasGoodSleep, opt => opt.MapFrom(s => s.HasGoodSleep))
                .ForMember(d => d.HasAdequateWater, opt => opt.MapFrom(s => s.HasAdequateWater))
                .ForMember(d => d.TookBreaks, opt => opt.MapFrom(s => s.TookBreaks));

            CreateMap<EnergyTrackingEntry, EnergyTrackingDto>()
                .ForMember(d => d.Level, opt => opt.MapFrom(s => s.Level));

            CreateMap<MoodTrackingEntry, MoodTrackingDto>()
                .ForMember(d => d.MoodLevel, opt => opt.MapFrom(s => s.MoodLevel));

            CreateMap<FocusSession, FocusSessionDto>()
                .ForMember(d => d.HasTaskLinked, opt => opt.MapFrom(s => s.HasTaskLinked))
                .ForMember(d => d.AveragePomodoroLength, opt => opt.MapFrom(s => s.AveragePomodoroLength))
                .ForMember(d => d.TotalFocusHours, opt => opt.MapFrom(s => s.TotalFocusHours));

            CreateMap<WellnessReport, WellnessReportDto>();

            // Planning mappings
            CreateMap<DailyReflection, DailyReflectionDto>()
                .ForMember(d => d.IsComplete, opt => opt.MapFrom(s => s.IsComplete))
                .ForMember(d => d.HasGratitude, opt => opt.MapFrom(s => s.HasGratitude))
                .ForMember(d => d.HasPriorities, opt => opt.MapFrom(s => s.HasPriorities));

            CreateMap<DailyIntention, DailyIntentionDto>()
                .ForMember(d => d.HasIntentions, opt => opt.MapFrom(s => s.HasIntentions))
                .ForMember(d => d.IntentionCount, opt => opt.MapFrom(s => s.IntentionCount));

            CreateMap<DailyQuote, DailyQuoteDto>()
                .ForMember(d => d.HasBeenViewed, opt => opt.MapFrom(s => s.HasBeenViewed))
                .ForMember(d => d.HasAuthor, opt => opt.MapFrom(s => s.HasAuthor));
        }
    }
}
