using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.TeamCollaboration.EntityFrameworkCore;

[ConnectionStringName(TeamCollaborationDbProperties.ConnectionStringName)]
public class TeamCollaborationDbContext : AbpDbContext<TeamCollaborationDbContext>, ITeamCollaborationDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public TeamCollaborationDbContext(DbContextOptions<TeamCollaborationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureTeamCollaboration();
    }
}
