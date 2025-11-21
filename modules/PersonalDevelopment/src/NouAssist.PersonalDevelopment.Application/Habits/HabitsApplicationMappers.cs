using System.Linq;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Entities;
using NouAssist.PersonalDevelopment.Habits.Habits.Dtos;
using NouAssist.PersonalDevelopment.Habits.Planning.Dtos;
using NouAssist.PersonalDevelopment.Habits.Routines.Dtos;
using NouAssist.PersonalDevelopment.Habits.Tracking.Dtos;
using NouAssist.PersonalDevelopment.Habits.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.PersonalDevelopment.Habits
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class HabitToHabitDtoMapper : MapperBase<Habit, HabitDto>
    {
        [MapperIgnoreTarget(nameof(HabitDto.IsMakeHabit))]
        [MapperIgnoreTarget(nameof(HabitDto.IsBreakHabit))]
        [MapperIgnoreTarget(nameof(HabitDto.HasSchedule))]
        [MapperIgnoreTarget(nameof(HabitDto.IsScheduledForToday))]
        public override partial HabitDto Map(Habit source);

        [MapperIgnoreTarget(nameof(HabitDto.IsMakeHabit))]
        [MapperIgnoreTarget(nameof(HabitDto.IsBreakHabit))]
        [MapperIgnoreTarget(nameof(HabitDto.HasSchedule))]
        [MapperIgnoreTarget(nameof(HabitDto.IsScheduledForToday))]
        public override partial void Map(Habit source, HabitDto destination);

        public override void AfterMap(Habit source, HabitDto destination)
        {
            destination.IsMakeHabit = source.IsMakeHabit;
            destination.IsBreakHabit = source.IsBreakHabit;
            destination.HasSchedule = source.HasSchedule;
            destination.IsScheduledForToday = source.IsScheduledForToday();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class HabitCueToHabitCueDtoMapper : MapperBase<HabitCue, HabitCueDto>
    {
        [MapperIgnoreTarget(nameof(HabitCueDto.CueTypeDescription))]
        public override partial HabitCueDto Map(HabitCue source);

        [MapperIgnoreTarget(nameof(HabitCueDto.CueTypeDescription))]
        public override partial void Map(HabitCue source, HabitCueDto destination);

        public override void AfterMap(HabitCue source, HabitCueDto destination)
        {
            destination.CueTypeDescription = source.GetCueTypeDescription();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class HabitResponseToHabitResponseDtoMapper : MapperBase<HabitResponse, HabitResponseDto>
    {
        [MapperIgnoreTarget(nameof(HabitResponseDto.HasDuration))]
        public override partial HabitResponseDto Map(HabitResponse source);

        [MapperIgnoreTarget(nameof(HabitResponseDto.HasDuration))]
        public override partial void Map(HabitResponse source, HabitResponseDto destination);

        public override void AfterMap(HabitResponse source, HabitResponseDto destination)
        {
            destination.HasDuration = source.HasDuration;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class HabitRewardToHabitRewardDtoMapper : MapperBase<HabitReward, HabitRewardDto>
    {
        [MapperIgnoreTarget(nameof(HabitRewardDto.HasCraving))]
        public override partial HabitRewardDto Map(HabitReward source);

        [MapperIgnoreTarget(nameof(HabitRewardDto.HasCraving))]
        public override partial void Map(HabitReward source, HabitRewardDto destination);

        public override void AfterMap(HabitReward source, HabitRewardDto destination)
        {
            destination.HasCraving = source.HasCraving;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class HabitStrategyToHabitStrategyDtoMapper : MapperBase<HabitStrategy, HabitStrategyDto>
    {
        [MapperIgnoreTarget(nameof(HabitStrategyDto.StrategyTypeDescription))]
        public override partial HabitStrategyDto Map(HabitStrategy source);

        [MapperIgnoreTarget(nameof(HabitStrategyDto.StrategyTypeDescription))]
        public override partial void Map(HabitStrategy source, HabitStrategyDto destination);

        public override void AfterMap(HabitStrategy source, HabitStrategyDto destination)
        {
            destination.StrategyTypeDescription = source.GetStrategyTypeDescription();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class HabitTrackingEntryToHabitTrackingDtoMapper : MapperBase<HabitTrackingEntry, HabitTrackingDto>
    {
        public override partial HabitTrackingDto Map(HabitTrackingEntry source);
        public override partial void Map(HabitTrackingEntry source, HabitTrackingDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class HabitMetricsToHabitMetricsDtoMapper : MapperBase<HabitMetrics, HabitMetricsDto>
    {
        public override partial HabitMetricsDto Map(HabitMetrics source);
        public override partial void Map(HabitMetrics source, HabitMetricsDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class RoutineToRoutineDtoMapper : MapperBase<Routine, RoutineDto>
    {
        [MapperIgnoreTarget(nameof(RoutineDto.HasSchedule))]
        [MapperIgnoreTarget(nameof(RoutineDto.IsEveryday))]
        [MapperIgnoreTarget(nameof(RoutineDto.TotalSteps))]
        [MapperIgnoreTarget(nameof(RoutineDto.EstimatedDurationMinutes))]
        public override partial RoutineDto Map(Routine source);

        [MapperIgnoreTarget(nameof(RoutineDto.HasSchedule))]
        [MapperIgnoreTarget(nameof(RoutineDto.IsEveryday))]
        [MapperIgnoreTarget(nameof(RoutineDto.TotalSteps))]
        [MapperIgnoreTarget(nameof(RoutineDto.EstimatedDurationMinutes))]
        public override partial void Map(Routine source, RoutineDto destination);

        public override void AfterMap(Routine source, RoutineDto destination)
        {
            destination.HasSchedule = source.HasSchedule;
            destination.IsEveryday = source.IsEveryday;
            destination.TotalSteps = source.TotalSteps;
            // EstimatedDurationMinutes is calculated in service, so we don't set it here
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class RoutineStepToRoutineStepDtoMapper : MapperBase<RoutineStep, RoutineStepDto>
    {
        [MapperIgnoreTarget(nameof(RoutineStepDto.HabitTitle))]
        [MapperIgnoreTarget(nameof(RoutineStepDto.IsHabitStep))]
        [MapperIgnoreTarget(nameof(RoutineStepDto.IsAdhocStep))]
        public override partial RoutineStepDto Map(RoutineStep source);

        [MapperIgnoreTarget(nameof(RoutineStepDto.HabitTitle))]
        [MapperIgnoreTarget(nameof(RoutineStepDto.IsHabitStep))]
        [MapperIgnoreTarget(nameof(RoutineStepDto.IsAdhocStep))]
        public override partial void Map(RoutineStep source, RoutineStepDto destination);

        public override void AfterMap(RoutineStep source, RoutineStepDto destination)
        {
            // HabitTitle is resolved in service, so we don't set it here
            destination.IsHabitStep = source.IsHabitStep;
            destination.IsAdhocStep = source.IsAdhocStep;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class RoutineCompletionToRoutineCompletionDtoMapper : MapperBase<RoutineCompletion, RoutineCompletionDto>
    {
        [MapperIgnoreTarget(nameof(RoutineCompletionDto.CompletionRate))]
        [MapperIgnoreTarget(nameof(RoutineCompletionDto.CompletedStepIds))]
        public override partial RoutineCompletionDto Map(RoutineCompletion source);

        [MapperIgnoreTarget(nameof(RoutineCompletionDto.CompletionRate))]
        [MapperIgnoreTarget(nameof(RoutineCompletionDto.CompletedStepIds))]
        public override partial void Map(RoutineCompletion source, RoutineCompletionDto destination);

        public override void AfterMap(RoutineCompletion source, RoutineCompletionDto destination)
        {
            destination.CompletionRate = source.CompletionRate;
            destination.CompletedStepIds = source.CompletedSteps.Select(cs => cs.StepId).ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class HealthTrackingEntryToHealthTrackingDtoMapper : MapperBase<HealthTrackingEntry, HealthTrackingDto>
    {
        [MapperIgnoreTarget(nameof(HealthTrackingDto.HasGoodSleep))]
        [MapperIgnoreTarget(nameof(HealthTrackingDto.HasAdequateWater))]
        [MapperIgnoreTarget(nameof(HealthTrackingDto.TookBreaks))]
        public override partial HealthTrackingDto Map(HealthTrackingEntry source);

        [MapperIgnoreTarget(nameof(HealthTrackingDto.HasGoodSleep))]
        [MapperIgnoreTarget(nameof(HealthTrackingDto.HasAdequateWater))]
        [MapperIgnoreTarget(nameof(HealthTrackingDto.TookBreaks))]
        public override partial void Map(HealthTrackingEntry source, HealthTrackingDto destination);

        public override void AfterMap(HealthTrackingEntry source, HealthTrackingDto destination)
        {
            destination.HasGoodSleep = source.HasGoodSleep;
            destination.HasAdequateWater = source.HasAdequateWater;
            destination.TookBreaks = source.TookBreaks;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class EnergyTrackingEntryToEnergyTrackingDtoMapper : MapperBase<EnergyTrackingEntry, EnergyTrackingDto>
    {
        [MapperIgnoreTarget(nameof(EnergyTrackingDto.Level))]
        public override partial EnergyTrackingDto Map(EnergyTrackingEntry source);

        [MapperIgnoreTarget(nameof(EnergyTrackingDto.Level))]
        public override partial void Map(EnergyTrackingEntry source, EnergyTrackingDto destination);

        public override void AfterMap(EnergyTrackingEntry source, EnergyTrackingDto destination)
        {
            destination.Level = source.Level;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class MoodTrackingEntryToMoodTrackingDtoMapper : MapperBase<MoodTrackingEntry, MoodTrackingDto>
    {
        [MapperIgnoreTarget(nameof(MoodTrackingDto.MoodLevel))]
        public override partial MoodTrackingDto Map(MoodTrackingEntry source);

        [MapperIgnoreTarget(nameof(MoodTrackingDto.MoodLevel))]
        public override partial void Map(MoodTrackingEntry source, MoodTrackingDto destination);

        public override void AfterMap(MoodTrackingEntry source, MoodTrackingDto destination)
        {
            destination.MoodLevel = source.MoodLevel;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class FocusSessionToFocusSessionDtoMapper : MapperBase<FocusSession, FocusSessionDto>
    {
        [MapperIgnoreTarget(nameof(FocusSessionDto.HasTaskLinked))]
        [MapperIgnoreTarget(nameof(FocusSessionDto.AveragePomodoroLength))]
        [MapperIgnoreTarget(nameof(FocusSessionDto.TotalFocusHours))]
        public override partial FocusSessionDto Map(FocusSession source);

        [MapperIgnoreTarget(nameof(FocusSessionDto.HasTaskLinked))]
        [MapperIgnoreTarget(nameof(FocusSessionDto.AveragePomodoroLength))]
        [MapperIgnoreTarget(nameof(FocusSessionDto.TotalFocusHours))]
        public override partial void Map(FocusSession source, FocusSessionDto destination);

        public override void AfterMap(FocusSession source, FocusSessionDto destination)
        {
            destination.HasTaskLinked = source.HasTaskLinked;
            destination.AveragePomodoroLength = source.AveragePomodoroLength;
            destination.TotalFocusHours = source.TotalFocusHours;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WellnessReportToWellnessReportDtoMapper : MapperBase<WellnessReport, WellnessReportDto>
    {
        public override partial WellnessReportDto Map(WellnessReport source);
        public override partial void Map(WellnessReport source, WellnessReportDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DailyReflectionToDailyReflectionDtoMapper : MapperBase<DailyReflection, DailyReflectionDto>
    {
        [MapperIgnoreTarget(nameof(DailyReflectionDto.IsComplete))]
        [MapperIgnoreTarget(nameof(DailyReflectionDto.HasGratitude))]
        [MapperIgnoreTarget(nameof(DailyReflectionDto.HasPriorities))]
        public override partial DailyReflectionDto Map(DailyReflection source);

        [MapperIgnoreTarget(nameof(DailyReflectionDto.IsComplete))]
        [MapperIgnoreTarget(nameof(DailyReflectionDto.HasGratitude))]
        [MapperIgnoreTarget(nameof(DailyReflectionDto.HasPriorities))]
        public override partial void Map(DailyReflection source, DailyReflectionDto destination);

        public override void AfterMap(DailyReflection source, DailyReflectionDto destination)
        {
            destination.IsComplete = source.IsComplete;
            destination.HasGratitude = source.HasGratitude;
            destination.HasPriorities = source.HasPriorities;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DailyIntentionToDailyIntentionDtoMapper : MapperBase<DailyIntention, DailyIntentionDto>
    {
        [MapperIgnoreTarget(nameof(DailyIntentionDto.HasIntentions))]
        [MapperIgnoreTarget(nameof(DailyIntentionDto.IntentionCount))]
        public override partial DailyIntentionDto Map(DailyIntention source);

        [MapperIgnoreTarget(nameof(DailyIntentionDto.HasIntentions))]
        [MapperIgnoreTarget(nameof(DailyIntentionDto.IntentionCount))]
        public override partial void Map(DailyIntention source, DailyIntentionDto destination);

        public override void AfterMap(DailyIntention source, DailyIntentionDto destination)
        {
            destination.HasIntentions = source.HasIntentions;
            destination.IntentionCount = source.IntentionCount;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DailyQuoteToDailyQuoteDtoMapper : MapperBase<DailyQuote, DailyQuoteDto>
    {
        [MapperIgnoreTarget(nameof(DailyQuoteDto.HasBeenViewed))]
        [MapperIgnoreTarget(nameof(DailyQuoteDto.HasAuthor))]
        public override partial DailyQuoteDto Map(DailyQuote source);

        [MapperIgnoreTarget(nameof(DailyQuoteDto.HasBeenViewed))]
        [MapperIgnoreTarget(nameof(DailyQuoteDto.HasAuthor))]
        public override partial void Map(DailyQuote source, DailyQuoteDto destination);

        public override void AfterMap(DailyQuote source, DailyQuoteDto destination)
        {
            destination.HasBeenViewed = source.HasBeenViewed;
            destination.HasAuthor = source.HasAuthor;
        }
    }
}
