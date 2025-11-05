using Microsoft.EntityFrameworkCore;
using NouAssist.Team.Aggregates;
using NouAssist.Team.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Team.EntityFrameworkCore
{
    [ConnectionStringName(TeamDbProperties.ConnectionStringName)]
    public interface ITeamDbContext : IEfCoreDbContext
    {
        DbSet<Team.Aggregates.Team> Teams { get; }
        DbSet<Role> Roles { get; }
        DbSet<TeamMember> TeamMembers { get; }
        DbSet<UserRole> UserRoles { get; }
    }
}
