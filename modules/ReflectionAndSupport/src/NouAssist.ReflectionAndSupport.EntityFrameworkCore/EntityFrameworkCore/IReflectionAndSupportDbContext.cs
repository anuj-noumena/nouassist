using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.EntityFrameworkCore;

[ConnectionStringName(ReflectionAndSupportDbProperties.ConnectionStringName)]
public interface IReflectionAndSupportDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
