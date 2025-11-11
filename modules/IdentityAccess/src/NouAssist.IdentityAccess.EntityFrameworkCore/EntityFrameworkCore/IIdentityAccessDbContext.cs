using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IdentityAccess.EntityFrameworkCore;

[ConnectionStringName(IdentityAccessDbProperties.ConnectionStringName)]
public interface IIdentityAccessDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
