using Microsoft.EntityFrameworkCore;
using NouAssist.PersonalDevelopment.Goals.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore;

[ConnectionStringName(PersonalDevelopmentDbProperties.ConnectionStringName)]
public interface IPersonalDevelopmentDbContext : IEfCoreDbContext
{
    DbSet<Goal> Goals { get; }
    DbSet<Achievement> Achievements { get; }
    DbSet<UserAchievement> UserAchievements { get; }

    // Aggregate Roots
    DbSet<Habit> Habits { get; }
    DbSet<HabitTrackingEntry> HabitTrackingEntries { get; }
    DbSet<Routine> Routines { get; }
    DbSet<RoutineCompletion> RoutineCompletions { get; }
    DbSet<HealthTrackingEntry> HealthTrackingEntries { get; }
    DbSet<EnergyTrackingEntry> EnergyTrackingEntries { get; }
    DbSet<MoodTrackingEntry> MoodTrackingEntries { get; }
    DbSet<FocusSession> FocusSessions { get; }
    DbSet<DailyReflection> DailyReflections { get; }
    DbSet<DailyIntention> DailyIntentions { get; }
    DbSet<DailyQuote> DailyQuotes { get; }

    // Entities (for direct querying if needed)
    DbSet<HabitCue> HabitCues { get; }
    DbSet<HabitResponse> HabitResponses { get; }
    DbSet<HabitReward> HabitRewards { get; }
    DbSet<HabitStrategy> HabitStrategies { get; }
    DbSet<RoutineStep> RoutineSteps { get; }
    DbSet<RoutineCompletedStep> RoutineCompletedSteps { get; }
}