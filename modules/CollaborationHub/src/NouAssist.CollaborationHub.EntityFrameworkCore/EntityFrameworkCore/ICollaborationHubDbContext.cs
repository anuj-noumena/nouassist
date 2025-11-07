using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.EntityFrameworkCore;

[ConnectionStringName(CollaborationHubDbProperties.ConnectionStringName)]
public interface ICollaborationHubDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
