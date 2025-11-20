using Microsoft.EntityFrameworkCore;
using NouAssist.PersonalDevelopment.Goals.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore;

[ConnectionStringName(PersonalDevelopmentDbProperties.ConnectionStringName)]
public class PersonalDevelopmentDbContext : AbpDbContext<PersonalDevelopmentDbContext>, IPersonalDevelopmentDbContext
{
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<UserAchievement> UserAchievements { get; set; }
    
    
    // Aggregate Roots
    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitTrackingEntry> HabitTrackingEntries { get; set; }
    public DbSet<Routine> Routines { get; set; }
    public DbSet<RoutineCompletion> RoutineCompletions { get; set; }
    public DbSet<HealthTrackingEntry> HealthTrackingEntries { get; set; }
    public DbSet<EnergyTrackingEntry> EnergyTrackingEntries { get; set; }
    public DbSet<MoodTrackingEntry> MoodTrackingEntries { get; set; }
    public DbSet<FocusSession> FocusSessions { get; set; }
    public DbSet<DailyReflection> DailyReflections { get; set; }
    public DbSet<DailyIntention> DailyIntentions { get; set; }
    public DbSet<DailyQuote> DailyQuotes { get; set; }

    // Entities
    public DbSet<HabitCue> HabitCues { get; set; }
    public DbSet<HabitResponse> HabitResponses { get; set; }
    public DbSet<HabitReward> HabitRewards { get; set; }
    public DbSet<HabitStrategy> HabitStrategies { get; set; }
    public DbSet<RoutineStep> RoutineSteps { get; set; }
    public DbSet<RoutineCompletedStep> RoutineCompletedSteps { get; set; }

    public PersonalDevelopmentDbContext(DbContextOptions<PersonalDevelopmentDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePersonalDevelopment();
    }
}
