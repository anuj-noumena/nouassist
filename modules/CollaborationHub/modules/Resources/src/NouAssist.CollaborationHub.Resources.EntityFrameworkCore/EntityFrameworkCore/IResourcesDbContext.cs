using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.Resources.EntityFrameworkCore;

[ConnectionStringName(ResourcesDbProperties.ConnectionStringName)]
public interface IResourcesDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
