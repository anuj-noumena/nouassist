using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.Journaling.EntityFrameworkCore;

[ConnectionStringName(JournalingDbProperties.ConnectionStringName)]
public interface IJournalingDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
