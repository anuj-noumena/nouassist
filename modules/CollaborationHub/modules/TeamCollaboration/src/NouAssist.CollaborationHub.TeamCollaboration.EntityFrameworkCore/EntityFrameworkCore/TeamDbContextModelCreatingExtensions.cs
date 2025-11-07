using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.CollaborationHub.TeamCollaboration.EntityFrameworkCore
{
    public static class TeamDbContextModelCreatingExtensions
    {
        public static void ConfigureTeam(
            this ModelBuilder builder,
            TeamDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(TeamDbContext).Assembly);
        }
    }
}
