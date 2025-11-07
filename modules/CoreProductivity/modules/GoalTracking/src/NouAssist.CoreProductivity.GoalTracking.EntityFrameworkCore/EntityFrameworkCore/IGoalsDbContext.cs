using Microsoft.EntityFrameworkCore;
using NouAssist.CoreProductivity.GoalTracking.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.GoalTracking.EntityFrameworkCore
{
    [ConnectionStringName(GoalsDbProperties.ConnectionStringName)]
    public interface IGoalsDbContext : IEfCoreDbContext
    {
        DbSet<Goal> Goals { get; }
        DbSet<Achievement> Achievements { get; }
        DbSet<UserAchievement> UserAchievements { get; }
    }
}
