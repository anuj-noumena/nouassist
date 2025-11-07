using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.HabitSystems.EntityFrameworkCore;

[ConnectionStringName(HabitSystemsDbProperties.ConnectionStringName)]
public interface IHabitSystemsDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
