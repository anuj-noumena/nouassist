using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.EntityFrameworkCore;

[ConnectionStringName(ActivityTrackingDbProperties.ConnectionStringName)]
public interface IActivityTrackingDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
