using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.EntityFrameworkCore;

[ConnectionStringName(BusinessOperationsDbProperties.ConnectionStringName)]
public interface IBusinessOperationsDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
