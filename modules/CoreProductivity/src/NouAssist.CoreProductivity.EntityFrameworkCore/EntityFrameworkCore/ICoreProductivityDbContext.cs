using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.EntityFrameworkCore;

[ConnectionStringName(CoreProductivityDbProperties.ConnectionStringName)]
public interface ICoreProductivityDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
