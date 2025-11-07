using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.GoalTracking.EntityFrameworkCore;

[ConnectionStringName(GoalTrackingDbProperties.ConnectionStringName)]
public interface IGoalTrackingDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
