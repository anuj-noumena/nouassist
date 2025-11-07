using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.TeamCollaboration.EntityFrameworkCore;

[ConnectionStringName(TeamCollaborationDbProperties.ConnectionStringName)]
public interface ITeamCollaborationDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
