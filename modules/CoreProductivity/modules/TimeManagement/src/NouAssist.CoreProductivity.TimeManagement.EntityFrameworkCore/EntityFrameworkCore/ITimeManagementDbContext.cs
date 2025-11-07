using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.TimeManagement.EntityFrameworkCore;

[ConnectionStringName(TimeManagementDbProperties.ConnectionStringName)]
public interface ITimeManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
