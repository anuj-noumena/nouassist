using Microsoft.EntityFrameworkCore;
using NouAssist.Team.Aggregates;
using NouAssist.Team.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Team.EntityFrameworkCore
{
    [ConnectionStringName(TeamDbProperties.ConnectionStringName)]
    public class TeamDbContext : AbpDbContext<TeamDbContext>, ITeamDbContext
    {
        public DbSet<Team.Aggregates.Team> Teams { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public TeamDbContext(DbContextOptions<TeamDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureTeam(this);
        }
    }
}
