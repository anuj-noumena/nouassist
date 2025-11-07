using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.SharedKernel.EntityFrameworkCore;

[ConnectionStringName(SharedKernelDbProperties.ConnectionStringName)]
public interface ISharedKernelDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
